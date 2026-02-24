using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml.Linq;

namespace ImgAnalyzer
{

    public class SamplesDB
    {
        private static readonly string connectionString = $"Data Source=Samples.db;Version=3;";



        public static void InitializeDatabase()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Создание таблицы Samples
                string createSamplesTable = @"
                CREATE TABLE IF NOT EXISTS Samples (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL UNIQUE,
                    Comment TEXT
                )";

                using (var command1 = new SQLiteCommand(createSamplesTable, connection))
                {
                    command1.ExecuteNonQuery();
                }

                // Создание таблицы ContainerBatches
                string createBatchesTable = @"
                CREATE TABLE IF NOT EXISTS ContainerBatches (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    SampleId INTEGER NOT NULL,
                    BatchType TEXT NOT NULL,
                    Comment TEXT,
                    Filenames TEXT NOT NULL,
                    FOREIGN KEY (SampleId) REFERENCES Samples(Id) ON DELETE SET NULL
                )";

                using (var command2 = new SQLiteCommand(createBatchesTable, connection))
                {
                    command2.ExecuteNonQuery();
                }

                // Создание индекса для быстрого поиска
                string createIndex = @"
                CREATE INDEX IF NOT EXISTS idx_batches_lookup 
                ON ContainerBatches(SampleId, BatchType)";

                using (var command3 = new SQLiteCommand(createIndex, connection))
                {
                    command3.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Возвращает список имен из таблицы Samples
        /// </summary>
        public static List<string> GetSamplesList()
        {
            var samples = new List<string>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name FROM Samples ORDER BY Name";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            samples.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return samples;
        }

        /// <summary>
        /// Возвращает Id элемента из таблицы Samples по имени, или -1 если такого имени нет
        /// </summary>
        public static int GetSampleId(string name)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM Samples WHERE Name = @name";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        public static bool UpdateSampleComment(int id, string comment)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE Samples SET Comment = @comment WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@comment", comment ?? "");

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
        public static string GetSampleName(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name FROM Samples WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToString(result) : "";
                }
            }


        }

        public static string GetSampleComment(int id)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Comment FROM Samples WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    var result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }

                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Возвращает список id элементов второй таблицы, у которых sample_id и batch_type совпадают с указанными
        /// </summary>
        public static List<int> GetBatchesId(int sampleId, string batchType)
        {
            var batchIds = new List<int>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id FROM ContainerBatches WHERE SampleId = @sampleId AND BatchType = @batchType ORDER BY Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sampleId", sampleId);
                    command.Parameters.AddWithValue("@batchType", batchType);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            batchIds.Add(reader.GetInt32(0));
                        }
                    }
                }
            }

            return batchIds;
        }

        /// <summary>
        /// Добавляет новый образец в таблицу Samples
        /// </summary>
        public static int AddSample(string name, string comment = "")
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Samples (Name, Comment) VALUES (@name, @comment); SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@comment", comment ?? "");

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Добавляет новую партию в таблицу ContainerBatches
        /// </summary>
        public static int AddContainerBatch(int sampleId, string batchType, List<string> filenames, string comment = "")
        {
            string filenamesJson = JsonSerializer.Serialize(filenames);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO ContainerBatches (SampleId, BatchType, Comment, Filenames) VALUES (@sampleId, @batchType, @comment, @filenames); SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sampleId", sampleId);
                    command.Parameters.AddWithValue("@batchType", batchType);
                    command.Parameters.AddWithValue("@comment", comment ?? "");
                    command.Parameters.AddWithValue("@filenames", filenamesJson);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Получает имена файлов для указанной партии
        /// </summary>
        public static List<string> GetBatchFilenames(int batchId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Filenames FROM ContainerBatches WHERE Id = @batchId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@batchId", batchId);

                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        string json = result.ToString();
                        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
                    }

                    return new List<string>();
                }
            }
        }

        public static string GetBatchName(int batchId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Name FROM ContainerBatches WHERE Id = @batchId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@batchId", batchId);

                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return result.ToString();
                    }
                    else { return ""; }

                }
            }

        }




        /// <summary>
        /// Проверяет существует ли образец с указанным именем
        /// </summary>
        /// 
        public static bool SampleExists(string name)
        {
            return GetSampleId(name) != -1;
        }

        /// <summary>
        /// Получает информацию о всех партиях для указанного образца и типа
        /// </summary>
        public static List<Tuple<int, string, List<string>>> GetBatchesInfo(int sampleId, string batchType)
        {
            var batches = new List<Tuple<int, string, List<string>>>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Comment, Filenames FROM ContainerBatches WHERE SampleId = @sampleId AND BatchType = @batchType ORDER BY Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sampleId", sampleId);
                    command.Parameters.AddWithValue("@batchType", batchType);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string comment = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string filenamesJson = reader.GetString(2);
                            var filenames = JsonSerializer.Deserialize<List<string>>(filenamesJson) ?? new List<string>();

                            batches.Add(Tuple.Create(id, comment, filenames));
                        }
                    }
                }
            }

            return batches;
        }

        /// <summary>
        /// Проверяет, есть ли в таблице ContainerBatches элементы, у которых Filenames содержит хотя бы одно имя из входного массива
        /// </summary>
        /// <param name="filenames">Массив имен файлов для проверки</param>
        /// <returns>id последней пачки в которой попался один из файлов</returns>
        public static int CheckFilenamesExist(string[] filenames, out int total_count)
        {
            var results = new List<(int, List<string>)>();

            total_count = 0;
            int result = -1;

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Filenames FROM ContainerBatches";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int batchId = reader.GetInt32(0);
                            string filenamesJson = reader.GetString(1);

                            try
                            {
                                var batchFilenames = JsonSerializer.Deserialize<List<string>>(filenamesJson) ?? new List<string>();

                                // Находим пересечение входного массива с файлами в текущей партии
                                var foundFiles = batchFilenames.Intersect(filenames).ToList();

                                if (foundFiles.Any())
                                {
                                    result = batchId;
                                    total_count++;
                                }
                            }
                            catch (JsonException)
                            {
                                // Пропускаем записи с некорректным JSON
                                continue;
                            }
                        }
                    }
                }
            }

            return result;
        }

    }


}
