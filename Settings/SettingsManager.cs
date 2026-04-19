using ImgAnalyzer.DialogForms;
using ImgAnalyzer.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.ComponentModel;
using ImgAnalyzer.Settings;
using System.Web;
using System.Runtime.CompilerServices;

namespace ImgAnalyzer
{
    public class SettingsManager
    {
        //public static SettingsManager Instance { get; private set; }

        public const string _showForm = "ShowForm";
        public static bool ShowForm { get; set; } = false;

        public const string _showFormOnFirstRequest = "ShowFormOnFirstRequest";
        public static bool ShowFormOnFirstRequest { get; set; } = false;

        public static void Initalize()
        {
            var connectionString = "Data Source=settings.db;Version=3;";
            _dbConnection = new SQLiteConnection(connectionString); 
            _tableName = "AppSettings";

            InitializeDatabase();

            var sd_ShowForm = SettingDefinition.CreateGlobal(_showForm, false, "Показывать значения при запросе из базы");
            var sd_SFOFR = SettingDefinition.CreateGlobal(_showFormOnFirstRequest, false, "Показывать значения при первом запросе из базы");
            GetSettingsFromDatabase(new List<SettingDefinition> { sd_ShowForm, sd_SFOFR });
            ShowForm = sd_ShowForm.GetValue<bool>();
            ShowFormOnFirstRequest = sd_SFOFR.GetValue<bool>();



        }

        private static  IDbConnection _dbConnection;
        private static  string _tableName;
        private static bool formRequest = true;


        /// <summary>
        /// Инициализация структуры базы данных (создание таблицы при необходимости)
        /// </summary>
        private static void InitializeDatabase()
        {
            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();

                // Создаем таблицу, если она не существует
                var createTableQuery = $@"
                CREATE TABLE IF NOT EXISTS {_tableName} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Owner TEXT,
                    Value TEXT,
                    ValueType TEXT,
                    Comment TEXT,
                    LastModified DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

                using (var command = _dbConnection.CreateCommand())
                {
                    command.CommandText = createTableQuery;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка инициализации базы данных настроек: {ex.Message}", ex);
            }
        }


        public static void RequestSettingList(List<SettingDefinition> settings, bool forceRequest = false)
        {

            GetSettingsFromDatabase(settings);
            if (forceRequest || ShowForm)
            {
                SettigRequestForm form = new SettigRequestForm(settings);
                form.ShowDialog();
                if (form.saveAfterUse) SaveSettingsToDatabase(settings);
            }
            else if (ShowFormOnFirstRequest)
            {
                List<SettingDefinition> old_list = new List<SettingDefinition>();
                List<SettingDefinition> new_list = new List<SettingDefinition>();

                foreach (var s in settings)
                {
                    if (SettingExist(s)) old_list.Add(s);
                    else new_list.Add(s);
                }

                if (new_list.Count > 0)
                {
                    SettigRequestForm form = new SettigRequestForm(new_list);
                    form.ShowDialog();
                    if (form.saveAfterUse) SaveSettingsToDatabase(new_list);    
                }
                GetSettingsFromDatabase(old_list);
            }
        }

        public static BindingList<SettingRecord> GetAllSettings(string ownerName = "", string searchText ="")
        {
            BindingList<SettingRecord> result = new BindingList<SettingRecord>();

            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();

                using (var transaction = _dbConnection.BeginTransaction())
                {
                    string where_statement = "";
                    if (ownerName.Length > 0 || searchText.Length > 0) where_statement += "WHERE ";
                    if (ownerName.Length > 0) where_statement += "Owner = @Owner ";
                    if (ownerName.Length > 0 && searchText.Length > 0) where_statement += "AND ";
                    if (searchText.Length > 0) where_statement += $"Name LIKE '%{searchText}%'";


                    var getQuery = $"SELECT * FROM {_tableName} {where_statement} ORDER BY Owner";
                    using (var getCommand = _dbConnection.CreateCommand())
                    {
                        getCommand.Transaction = transaction;
                        getCommand.CommandText = getQuery;
                        if (ownerName.Length >0) AddParameter(getCommand, "@Owner", ownerName);

                        using (var reader = getCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SettingRecord record = new SettingRecord();
                                record.Id = reader.GetInt32(0);
                                record.Name = reader.GetString(1);
                                record.Owner = reader.GetString(2);
                                record.Value = reader.GetString(3);
                                record.Datatype = reader.GetString(4);
                                record.Comment = reader.GetString(5);
                                record.modified = reader.GetDateTime(6);

                                result.Add(record);

                            }
                        }
                    }
                    

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка синхронизации настроек: {ex.Message}", ex);
            }
            return result;
        }

        /// <summary>
        /// Синхронизация зарегистрированных настроек с базой данных
        /// </summary>
        public static void GetSettingsFromDatabase(IEnumerable<SettingDefinition> settings)
        {

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            foreach (var setting in settings)
            {
                if (string.IsNullOrWhiteSpace(setting.Name))
                    throw new ArgumentException("Имя настройки не может быть пустым");

            }

            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();

                using (var transaction = _dbConnection.BeginTransaction())
                {
                    foreach (var setting in settings)
                    {
                        // Проверяем, существует ли настройка в базе
                        string valueType = setting.ValueType.FullName;
                        var checkQuery = $"SELECT COUNT(*) FROM {_tableName} WHERE Name = @Name AND Owner = @sender AND ValueType = '{valueType}'";
                        using (var checkCommand = _dbConnection.CreateCommand())
                        {
                            checkCommand.Transaction = transaction;
                            checkCommand.CommandText = checkQuery;
                            AddParameter(checkCommand, "@Name", setting.Name);
                            AddParameter(checkCommand, "@sender", setting.Owner);

                            var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;

                            if (!exists)
                            {
                                // Добавляем настройку с значением по умолчанию
                                var insertQuery = $@"
                                INSERT INTO {_tableName} (Name, Owner, Value, ValueType, Comment) 
                                VALUES (@Name, @Owner, @Value, @ValueType, @Comment)";

                                using (var insertCommand = _dbConnection.CreateCommand())
                                {
                                    insertCommand.Transaction = transaction;
                                    insertCommand.CommandText = insertQuery;

                                    AddParameter(insertCommand, "@Name", setting.Name);
                                    AddParameter(insertCommand, "@Owner", setting.Owner);
                                    AddParameter(insertCommand, "@Value", ConvertToString(setting.DefaultValue));
                                    AddParameter(insertCommand, "@ValueType", setting.ValueType.FullName);
                                    AddParameter(insertCommand, "@Comment", setting.Comment);

                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                            else 
                            {
                                var updateQuery = $@"
                                UPDATE {_tableName} 
                                SET Comment = @Comment 
                                WHERE Name = @Name AND Owner = @Owner AND ValueType = @ValueType";
                                if (setting.Comment?.Length > 0)
                                    using (var updateCommand = _dbConnection.CreateCommand())
                                    {
                                        updateCommand.Transaction = transaction;
                                        updateCommand.CommandText = updateQuery;

                                        AddParameter(updateCommand, "@Name", setting.Name);
                                        AddParameter(updateCommand, "@Owner", setting.Owner);
                                        AddParameter(updateCommand, "@ValueType", setting.ValueType.FullName);
                                        AddParameter(updateCommand, "@Comment", setting.Comment);

                                        updateCommand.ExecuteNonQuery();
                                    }

                                var request_query = $"SELECT Value FROM {_tableName} WHERE Name = @Name";
                                using (var command = _dbConnection.CreateCommand())
                                {
                                    command.CommandText = request_query;
                                    AddParameter(command, "@Name", setting.Name);

                                    var result = command.ExecuteScalar();

                                    if (result == null || result == DBNull.Value || !setting.ConvertFromString(result.ToString()))
                                    {
                                        // Возвращаем значение по умолчанию
                                        setting.SetDefault();
                                    }

                                }
                            }
                        }
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка синхронизации настроек: {ex.Message}", ex);
            }
        }

        public static void SaveSettingsToDatabase(List<SettingDefinition> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            foreach (var setting in settings)
            {
                if (string.IsNullOrWhiteSpace(setting.Name))
                    throw new ArgumentException("Имя настройки не может быть пустым");

            }

            try
            {
                if (_dbConnection.State != ConnectionState.Open) _dbConnection.Open();

                foreach (var setting in settings)
                {
                    // Проверяем, существует ли настройка в базе
                    var checkQuery = $"SELECT COUNT(*) FROM {_tableName} WHERE Name = @Name AND Owner = @sender";
                    bool exist = false;
                    using (var checkCommand = _dbConnection.CreateCommand())
                    {
                        checkCommand.CommandText = checkQuery;
                        AddParameter(checkCommand, "@Name", setting.Name);
                        AddParameter(checkCommand, "@sender", setting.Owner);
                        exist = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                    }
                    if (!exist)
                    {
                        // Добавляем настройку с значением по умолчанию
                        var insertQuery = $@"
                                INSERT INTO {_tableName} (Name, Owner, Value, ValueType, Comment) 
                                VALUES (@Name, @Owner, @Value, @ValueType, @Comment)";

                        using (var insertCommand = _dbConnection.CreateCommand())
                        {
                            insertCommand.CommandText = insertQuery;

                            AddParameter(insertCommand, "@Name", setting.Name);
                            AddParameter(insertCommand, "@Owner", setting.Owner);
                            AddParameter(insertCommand, "@Value", ConvertToString(setting.DefaultValue));
                            AddParameter(insertCommand, "@ValueType", setting.ValueType.FullName);
                            AddParameter(insertCommand, "@Comment", setting.Comment);

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Обновляем тип и комментарий, если они изменились
                        var updateQuery = $@"
                                UPDATE {_tableName} 
                                SET ValueType = @ValueType, Value = @Value ,Comment = @Comment 
                                WHERE Name = @Name AND Owner = @Owner";

                        using (var updateCommand = _dbConnection.CreateCommand())
                        {
                            updateCommand.CommandText = updateQuery;

                            AddParameter(updateCommand, "@Value", ConvertToString(setting.Value));

                            AddParameter(updateCommand, "@Name", setting.Name);
                            AddParameter(updateCommand, "@Owner", setting.Owner);
                            AddParameter(updateCommand, "@ValueType", setting.ValueType.FullName);
                            AddParameter(updateCommand, "@Comment", setting.Comment);

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка синхронизации настроек: {ex.Message}", ex);
            }



        }

        public static bool SettingExist(SettingDefinition setting)
        {
            if (_dbConnection.State != ConnectionState.Open) _dbConnection.Open();
            var checkQuery = $"SELECT COUNT(*) FROM {_tableName} WHERE Name = @Name AND Owner = @sender";
            using (var checkCommand = _dbConnection.CreateCommand())
            {
                checkCommand.CommandText = checkQuery;
                AddParameter(checkCommand, "@Name", setting.Name);
                AddParameter(checkCommand, "@sender", setting.Owner);
                return Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
            }
        }


        public List<SettingDefinition> GetFirstRequests(List<SettingDefinition> settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            foreach (var setting in settings)
            {
                if (string.IsNullOrWhiteSpace(setting.Name))
                    throw new ArgumentException("Имя настройки не может быть пустым");
            }

            List<SettingDefinition> result = new List<SettingDefinition>();

            try
            {
                foreach (var setting in settings)
                {
                    if (!SettingExist(setting)) result.Add(setting);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка поиска настроек: {ex.Message}", ex);
            }
            return result;
        }

        public void EditSetting(int id, string newValue)
        { 
            
        
        
        
        
        }


        private static void SaveSettingToDatabase(string name, object Value, string owner = "", string comment = "")
        { 
            
            SettingDefinition sd = new SettingDefinition(name, Value.ToString());
            if (owner?.Length > 0) sd.Owner = owner; else sd.Owner = "global";
            if (comment?.Length > 0) sd.Comment = comment;
            SaveSettingsToDatabase(new List<SettingDefinition> { sd });
        }

        private static bool GetSettingFromDatabase(string name, ref object value, string owner = "")
        {
            bool result = false;

            try
            {
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();

                using (var transaction = _dbConnection.BeginTransaction())
                {

                    // Проверяем, существует ли настройка в базе
                    string valueType = value.GetType().FullName;
                    var checkQuery = $"SELECT COUNT(*) FROM {_tableName} WHERE Name = @Name AND Owner = @sender AND ValueType = '{valueType}'";
                    using (var checkCommand = _dbConnection.CreateCommand())
                    {
                        checkCommand.Transaction = transaction;
                        checkCommand.CommandText = checkQuery;
                        AddParameter(checkCommand, "@Name", name);
                        AddParameter(checkCommand, "@sender", owner);

                        var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;

                        if (!exists)
                        {
                            return false;
                        }
                        else
                        {
                            var updateQuery = $@"
                                UPDATE {_tableName} 
                                SET Comment = @Comment 
                                WHERE Name = @Name AND Owner = @Owner AND ValueType = '{valueType}'";

                            using (var updateCommand = _dbConnection.CreateCommand())
                            {
                                updateCommand.Transaction = transaction;
                                updateCommand.CommandText = updateQuery;

                                AddParameter(updateCommand, "@Name", name);
                                AddParameter(updateCommand, "@Owner", owner);
                                updateCommand.ExecuteNonQuery();
                            }


                            var request_query = $"SELECT Value FROM {_tableName} WHERE Name = @Name";
                            using (var command = _dbConnection.CreateCommand())
                            {
                                command.CommandText = request_query;
                                AddParameter(command, "@Name", name);

                                var output = command.ExecuteScalar();

                                if (output == null || output == DBNull.Value)
                                {
                                    string s = output.ToString();
                                    if (value is bool) value = bool.Parse(s);
                                    else if (value is int) value = int.Parse(s);
                                    else if (value is double) value = double.Parse(s);
                                    else if (value is decimal) value = decimal.Parse(s);
                                    else if (value is DateTime) value = DateTime.Parse(s);
                                    else if (value is string) value = s;
                                    else return false;
                                }
                                result = true;
                            }
                        }
                    }
                    transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return result;
        }




        /// <summary>
        /// Конвертация значения в строку для хранения в базе
        /// </summary>
        private static string ConvertToString(object value)
        {
            if (value == null)
                return string.Empty;

            if (value is DateTime dateTime)
                return dateTime.ToString("o"); // ISO 8601 формат

            return value.ToString();
        }



        /// <summary>
        /// Добавление параметра к команде
        /// </summary>
        private static void AddParameter(IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public static void Dispose()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }




    }
}
