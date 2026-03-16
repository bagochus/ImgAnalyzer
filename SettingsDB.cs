using ImgAnalyzer._2D;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.ApplicationServices;
using NetTopologySuite.Noding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ImgAnalyzer
{
    public static class SettingsDB
    {
        public static string databaseFile = "Settings.db";
        //public static string databaseFile = "C:\\ProgramData\\ISP\\ImgAnalyzer\\Settings.db";
        private static string connectionString = $"Data Source={databaseFile};Version=3;";


        public static void InitializeSettingsDB()
        {
            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createTables = @"
                CREATE TABLE IF NOT EXISTS StringValues (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL UNIQUE,
                    value TEXT NOT NULL,
                    comment TEXT
                ); 

                CREATE TABLE IF NOT EXISTS DoubleValues (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    name TEXT NOT NULL UNIQUE,
                    value DOUBLE NOT NULL,
                    comment TEXT
                )";

                    var command = new SQLiteCommand(createTables, connection);
                    command.ExecuteNonQuery();
                }

            }
        }

        public static void CreateIfNotExistString(string name, string value, string comment)
        {
            if (ExistsString(name)) return;
            string sql = @"
            INSERT INTO StringValues (name, value, comment)
            valueS (@name, @value, @comment)";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@comment", comment ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool ExistsString(string name)
        {
            string sql = "SELECT COUNT(1) FROM StringValues WHERE name = @name";
            
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection)) 
                {
                    command.Parameters.AddWithValue("@name", name);
                    var count = Convert.ToInt64(command.ExecuteScalar());
                    return count > 0;

                }
            }
        }

        public static void UpadateString(string name, string value)
        {
            string sql = "UPDATE StringValues SET value = @value WHERE name = @name";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                }
            }


        }




        public static bool GetString (string name, out string value)
        {
            string sql = "SELECT * FROM StringValues WHERE name = @name;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            value = Convert.ToString(reader["value"]);
                            //string comment = Convert.ToString(reader["comment"]);
                            return true;
                        }
                    }
                }
            }

            value = Interaction.InputBox($"Введите значение праметра {name}",
            "Параметр не найден",
            "");
            if (value != "") UpadateString(name, value);

            return value != "";

        }

        //Double part


        public static void CreateIfNotExistDouble(string name, double value, string comment)
        {
            if (ExistsDouble(name)) return;
            string sql = @"
            INSERT INTO DoubleValues (name, value, comment)
            valueS (@name, @value, @comment)";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@value", value);
                    command.Parameters.AddWithValue("@comment", comment ?? (object)DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static bool ExistsDouble(string name)
        {
            string sql = "SELECT COUNT(1) FROM DoubleValues WHERE name = @name";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    var count = Convert.ToInt64(command.ExecuteScalar());
                    return count > 0;

                }
            }
        }

        public static void UpadateDouble(string name, double value)
        {
            string sql = "UPDATE DoubleValues SET value = @value WHERE name = @name";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@value", value);
                    command.ExecuteNonQuery();
                }
            }


        }

        public static bool GetDouble(string name, out double value)
        {
            string sql = "SELECT * FROM DoubleValues WHERE name = @name;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            value = Convert.ToDouble(reader["value"]);
                            //string comment = Convert.ToString(reader["comment"]);
                            return true;
                        }
                    }
                }
            }

            string userInput = Interaction.InputBox($"Введите значение праметра {name}",
            "Параметр не найден",
            "");
            userInput.Replace('.',',');

            if (Double.TryParse(userInput, out value)) 
            {
                CreateIfNotExistDouble(name, value, "");
                return true;
            }
            else value = Double.NaN;

            return false;

        }












    }
}
