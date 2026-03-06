using ImgAnalyzer.DialogForms;
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

namespace ImgAnalyzer
{
    public class SettingsManager
    {
        //public static SettingsManager Instance { get; private set; }

        public static void Initalize()
        {
            var connectionString = "Data Source=settings.db;Version=3;";
            _dbConnection = new SQLiteConnection(connectionString); 
            _tableName = "AppSettings";

            InitializeDatabase();

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
                    Name TEXT PRIMARY KEY,
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
            if (forceRequest && true)
            {
                SettigRequestForm form = new SettigRequestForm(settings);
                form.ShowDialog();
                if (form.saveAfterUse) SaveSettingsToDatabase(settings);
            }




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
                        var checkQuery = $"SELECT COUNT(*) FROM {_tableName} WHERE Name = @Name AND Owner = @sender";
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
                                SET ValueType = @ValueType, Comment = @Comment 
                                WHERE Name = @Name";

                                using (var updateCommand = _dbConnection.CreateCommand())
                                {
                                    updateCommand.Transaction = transaction;
                                    updateCommand.CommandText = updateQuery;

                                    AddParameter(updateCommand, "@Name", setting.Name);
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
                if (_dbConnection.State != ConnectionState.Open)
                    _dbConnection.Open();

                using (var transaction = _dbConnection.BeginTransaction())
                {
                    foreach (var setting in settings)
                    {
                        // Проверяем, существует ли настройка в базе
                        var checkQuery = $"SELECT COUNT(*) FROM {_tableName} WHERE Name = @Name AND Owner = @sender";
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
                                // Обновляем тип и комментарий, если они изменились
                                var updateQuery = $@"
                                UPDATE {_tableName} 
                                SET ValueType = @ValueType, Value = @Value ,Comment = @Comment 
                                WHERE Name = @Name AND Owner = @Owner";

                                using (var updateCommand = _dbConnection.CreateCommand())
                                {
                                    updateCommand.Transaction = transaction;
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

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Ошибка синхронизации настроек: {ex.Message}", ex);
            }

        
        
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
