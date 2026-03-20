using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics.Eventing.Reader;

namespace ImgAnalyzer
{


    public class SamplesDB
    {

        public static readonly string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
            "\\ImgAnalyzer\\SamplesDatabase_ver_A.db";
        private static readonly string connectionString = $"Data Source={filename};Version=3;";


        public static void InitializeDatabase()
        {
            if (!File.Exists(filename))
            {
                SQLiteConnection.CreateFile(filename);


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
                    SampleId INTEGER ,
                    BatchType TEXT NOT NULL,
                    Comment TEXT,
                    Filenames TEXT NOT NULL,
                    Width INTEGER DEFAULT 0,
                    Height INTEGER DEFAULT 0,
                    Count INTEGER DEFAULT -1,
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
        /// Возвращает список id элементов второй таблицы, у которых sample_name и batch_type совпадают с указанными
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
        public static int AddContainerBatch(int sampleId, string batchType, List<string> filenames, string comment = "", string Name = "")
        {
            string filenamesJson = JsonSerializer.Serialize(filenames);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string _name = Name;
                if (Name == "")
                {
                    int prev_id = 0;
                    string query_id = "SELECT last_insert_rowid();";
                    using (var command = new SQLiteCommand(query_id, connection))
                    {
                        prev_id = Convert.ToInt32(command.ExecuteScalar());
                    }
                    _name = batchType + (prev_id + 1).ToString();
                }

                string query = "INSERT INTO ContainerBatches (SampleId, Name, BatchType, Comment, Filenames)" +
                    " VALUES (@sampleId, @Name, @batchType, @comment, @filenames); SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sampleId", sampleId);
                    command.Parameters.AddWithValue("@batchType", batchType);
                    command.Parameters.AddWithValue("@comment", comment ?? "");
                    command.Parameters.AddWithValue("@filenames", filenamesJson);
                    command.Parameters.AddWithValue("@Name", _name);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public static int AddContainerBatch(ContainerBatch batch)
        {
            string filenamesJson = JsonSerializer.Serialize(batch.Filenames);

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();


                string _name = batch.Name;
                if (_name == "")
                {
                    int prev_id = 0;
                    string query_id = "SELECT last_insert_rowid();";
                    using (var command = new SQLiteCommand(query_id, connection))
                    {
                        prev_id = Convert.ToInt32(command.ExecuteScalar());
                    }
                    _name = batch.BatchType + (prev_id + 1).ToString();
                }

                string query = "INSERT INTO ContainerBatches (SampleId, Name, BatchType, Comment, Filenames, Width, Height, Count)" +
                    " VALUES (@sampleId, @Name, @batchType, @comment, @filenames, @width, @height, @Count); SELECT last_insert_rowid();";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@sampleId", batch.SampleId);
                    command.Parameters.AddWithValue("@Name", _name);
                    command.Parameters.AddWithValue("@batchType", batch.BatchType);
                    command.Parameters.AddWithValue("@comment", batch.comment ?? "");
                    command.Parameters.AddWithValue("@filenames", filenamesJson);
                    command.Parameters.AddWithValue("@width", batch.Width);
                    command.Parameters.AddWithValue("@height", batch.Height);
                    command.Parameters.AddWithValue("@Count", batch.Count);


                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public static bool UpdateContainerBatch(int id, ContainerBatch batch)
        {
            if (batch == null) return false;
            if (id < 0) return false;   

            bool result = true;

            try 
            {
                string filenamesJson = JsonSerializer.Serialize(batch.Filenames);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string _name = batch.Name;


                    string query = "UPDATE ContainerBatches" +
                        " SET Name = @Name," +
                        " Filenames = @filenames," +
                        " Comment = @comment" +
                        " Count = @Count" +
                        " Width = @width" +
                        " Height = @height" +
                        " WHERE Id = @Id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", batch.Name);
                        command.Parameters.AddWithValue("@comment", batch.comment ?? "");
                        command.Parameters.AddWithValue("@filenames", filenamesJson);
                        command.Parameters.AddWithValue("@width", batch.Width);
                        command.Parameters.AddWithValue("@height", batch.Height);
                        command.Parameters.AddWithValue("@Count", batch.Count);
                        int rowsAffected = command.ExecuteNonQuery();

                        return (rowsAffected == 1);
                    }
                }
            }
            catch { result = false; }
            return result;
        }

        public static bool UpdateBatchFilenames(int id, ContainerBatch batch)
        {
            if (batch == null) return false;
            if (id < 0) return false;

            bool result = true;

            try
            {
                string filenamesJson = JsonSerializer.Serialize(batch.Filenames);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string _name = batch.Name;


                    string query = "UPDATE ContainerBatches" +
                        " SET Filenames = @filenames" +
                        " Count = @Count" +
                        " WHERE Id = @Id";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Count", batch.Count);
                        command.Parameters.AddWithValue("@filenames", filenamesJson);
                        int rowsAffected = command.ExecuteNonQuery();
                        return (rowsAffected == 1);
                    }
                }
            }
            catch { result = false; }
            return result;
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



        public static ContainerBatch GetBatch(int id)
        {
            var batch = new ContainerBatch();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Id, Name, Comment, Filenames FROM ContainerBatches WHERE Id = @Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            batch.id = reader.GetInt32(0);
                            batch.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                            string comment = reader.IsDBNull(2) ? "" : reader.GetString(2);
                            string filenamesJson = reader.GetString(3);

                            var filenames = JsonSerializer.Deserialize<List<string>>(filenamesJson) ?? new List<string>();
                            batch.LocateImageBatch(filenames.ToArray());
                            batch.comment = comment;

                        }
                    }
                }
            }

            return batch;

        }

        public static List<BatchHeader> GetBatchHeaders()
        {
            var batches = new List<BatchHeader>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                //string query = "SELECT Id, Comment, Filenames FROM ContainerBatches WHERE SampleId = @sampleId AND BatchType = @batchType ORDER BY Id";
                string query = "SELECT Id, Name, SampleId, BatchType, Width, Height, Count " +
                    "FROM ContainerBatches ORDER BY Id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BatchHeader header = new BatchHeader();

                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            int sample_id = reader.GetInt32(2);
                            string batchType = reader.GetString(3);


                            int width = reader.GetInt32(4);
                            int height = reader.GetInt32(5);
                            int count = reader.GetInt32(6);

                            string sample_name = GetSampleName(sample_id);

                            header.Name = name;
                            header.Type = batchType;
                            header.Sample = sample_name;
                            header.Width = width;
                            header.Height = height;
                            header.Count = count;
                            header.id = id;
                            batches.Add(header);
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


        /// <summary>
        /// Проверяет, существует ли запись в таблице ContainerBatches с указанными Name и SampleId
        /// </summary>
        /// <param name="name">Название партии</param>
        /// <param name="sampleId">ID образца</param>
        /// <returns>true если запись существует, false если нет</returns>
        public static bool ContainerBatchExists(string name, int sampleId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM ContainerBatches WHERE Name = @name AND SampleId = @sampleId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@sampleId", sampleId);

                    long count = (long)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Добавляет текст в конец комментария для указанной партии с новой строки
        /// </summary>
        /// <param name="id">ID партии</param>
        /// <param name="text">Текст для добавления</param>
        /// <returns>true если обновление успешно, false если партия не найдена</returns>
        public static bool AppendBatchCommentNewLine(int id, string text)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = @"
            UPDATE ContainerBatches 
            SET Comment = CASE 
                WHEN Comment IS NULL OR Comment = '' THEN @text
                ELSE Comment || char(13) || char(10) || @text
            END
            WHERE Id = @id";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@text", text);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }



        public static bool UpdateBatchComment(int id, string comment)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE ContainerBatches SET Comment = @comment WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@comment", comment ?? "");

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }


        }
    }

}
