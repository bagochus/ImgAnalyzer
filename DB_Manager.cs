using ImgAnalyzer._2D;
using Microsoft.Data.Sqlite;
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
    internal class DB_Manager
    {

        private static string databaseFile = "Sessions.db";
        private static string connectionString = $"Data Source={databaseFile};Version=3;";




        

       

        public static void InitializeDatabase()
        {
            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string createTables = @"
                CREATE TABLE IF NOT EXISTS Containers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    filename TEXT NOT NULL,
                    image_group TEXT
                );

                CREATE TABLE IF NOT EXISTS Transformations (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    bl_x DOUBLE NOT NULL,
                    bl_y DOUBLE NOT NULL,
                    tl_x DOUBLE NOT NULL,
                    tl_y DOUBLE NOT NULL,
                    tr_x DOUBLE NOT NULL,
                    tr_y DOUBLE NOT NULL,
                    width INTEGER NOT NULL,
                    height INTEGER NOT NULL,
                    full_ct_filename, TEXT
                );

                CREATE TABLE IF NOT EXISTS ImageBatches (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    InnerID INTEGER NOT NULL,
                    Transform_id INTEGER,
                    filenames_json TEXT,
                    FOREIGN KEY (Transform_id) REFERENCES Transformations(Id) ON DELETE SET NULL
                );

                CREATE TABLE IF NOT EXISTS XAxisSettings (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Label TEXT NOT NULL,
                    Units TEXT NOT NULL,
                    Xstart DOUBLE NOT NULL,
                    Xstep DOUBLE NOT NULL
                );

                CREATE TABLE IF NOT EXISTS UserSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    BatchA_id INTEGER,
                    BatchB_id INTEGER,
                    BatchC_id INTEGER,
                    FOREIGN KEY (BatchA_id) REFERENCES ImageBatches(Id) ON DELETE SET NULL
                    FOREIGN KEY (BatchB_id) REFERENCES ImageBatches(Id) ON DELETE SET NULL
                    FOREIGN KEY (BatchC_id) REFERENCES ImageBatches(Id) ON DELETE SET NULL
                );
                    

                CREATE TABLE IF NOT EXISTS ContainersSessions (
                    session_id INTEGER NOT NULL,
                    container_id INTEGER NOT NULL,
                    PRIMARY KEY (session_id, container_id),
                    FOREIGN KEY (session_id) REFERENCES UserSessions(Id) ON DELETE CASCADE,
                    FOREIGN KEY (container_id) REFERENCES Containers(Id) ON DELETE CASCADE
                );

                CREATE TABLE IF NOT EXISTS AxisSessions (
                    session_id INTEGER NOT NULL,
                    xaxis_id INTEGER NOT NULL,
                    PRIMARY KEY (session_id, xaxis_id),
                    FOREIGN KEY (session_id) REFERENCES UserSessions(Id) ON DELETE CASCADE,
                    FOREIGN KEY (xaxis_id) REFERENCES XAxisSettings(Id) ON DELETE CASCADE
                )";









                    var command = new SQLiteCommand(createTables, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public static void SaveProfile(string profilename)
        {
            int batchA_id = 0;
            int batchB_id = 0;
            int batchC_id = 0;
            int session_id = 0;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();



                if (ImageManager.Batch_A().Count > 0) batchA_id = SaveImageBatch(ImageManager.Batch_A());
                if (ImageManager.Batch_B().Count > 0) batchB_id = SaveImageBatch(ImageManager.Batch_B());
                if (ImageManager.Batch_C().Count > 0) batchC_id = SaveImageBatch(ImageManager.Batch_C());


                string sql = @"INSERT INTO UserSessions
                     (Name, BatchA_id, BatchB_id, BatchC_id)
                     VALUES (@Name, @BatchA_id, @BatchB_id, @BatchC_id); 
                    SELECT last_insert_rowid()";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {   
                    command.Parameters.AddWithValue("@Name", profilename);
                    command.Parameters.AddWithValue("@BatchA_id", batchA_id);
                    command.Parameters.AddWithValue("@BatchB_id", batchB_id);
                    command.Parameters.AddWithValue("@BatchC_id", batchC_id);
                    session_id = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            if (session_id == 0) return;

            foreach (var c in DataManager_2D.containers) SaveContainer(c, session_id);  
            SaveXAxisSettings(session_id);
        }

        public static void LoadProfile (string  profilename)
        {
            int session_id = 0;
            
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM UserSessions WHERE Name = @Name";
                string sql2 = "SELECT * FROM ContainersSessions WHERE session_id = @session_id";
                string sql3 = "SELECT * FROM AxisSessions WHERE session_id = @session_id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", profilename);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            int batch_id = Convert.ToInt32(reader["BatchA_id"]);
                            if (batch_id != 0) ImageManager.Stacks[0] = LoadImageBatch(batch_id);
                            batch_id = Convert.ToInt32(reader["BatchB_id"]);
                            if (batch_id != 0) ImageManager.Stacks[1] = LoadImageBatch(batch_id);
                            batch_id = Convert.ToInt32(reader["BatchC_id"]);
                            if (batch_id != 0) ImageManager.Stacks[2] = LoadImageBatch(batch_id);
                            session_id = Convert.ToInt32(reader["Id"]);
                        }
                    }
                }

                DataManager_2D.ClearContainers();
                using (SQLiteCommand command = new SQLiteCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@session_id", session_id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            int container_id = Convert.ToInt32(reader["container_id"]);
                            DataManager_2D.containers.Add(LoadContainer(container_id));
                        }
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand(sql3, connection))
                {
                    command.Parameters.AddWithValue("@session_id", session_id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            int xaxis_id = Convert.ToInt32(reader["xaxis_id"]);
                            LoadXAxisSettings(xaxis_id);
                        }
                    }
                }





            }



        }

        public static void LoadCT(string profilename)
        {
            int[] batch_id = { 0,0,0};
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM UserSessions WHERE Name = @Name";
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", profilename);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            batch_id[0] = Convert.ToInt32(reader["BatchA_id"]);
                            batch_id[1] = Convert.ToInt32(reader["BatchB_id"]);
                            batch_id[2] = Convert.ToInt32(reader["BatchC_id"]);
                        }
                    }
                }

                string sql2 = "SELECT * FROM ImageBatches WHERE Id = @Id";
                for (int i = 0; i < batch_id.Length; i++)
                {
                    if (batch_id[i] == 0) continue;
                    using (SQLiteCommand command = new SQLiteCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@Id", batch_id[i]);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int ct_id = Convert.ToInt32(reader["Transform_id"]);
                                if (ct_id != 0) ImageManager.Batch(i).coordinateTransformation = LoadTransformation(ct_id);
                            }

                        }
                    }
                }
            }
        }

        public static int SaveTransformation(CoordinateTransformation ct)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string filename = "";

                connection.Open();
                if (ct.FullFieldCalculated || ct.FulCTFilename != "")
                {
                    if (ct.FulCTFilename != "") filename = ct.FulCTFilename;
                    else
                    {
                    filename = "transformation/fullct_" + DateTime.Now.Ticks + ".bin";
                    ct.SaveFullField(filename);
                    }

                }


                string sql = @"INSERT INTO Transformations 
                     (bl_x, bl_y, tl_x, tl_y, tr_x, tr_y, width, height, full_ct_filename)
                     VALUES (@bl_x, @bl_y, @tl_x, @tl_y, @tr_x, @tr_y, @width, @height, @full_ct_filename); 
                    SELECT last_insert_rowid()";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@bl_x", ct.point_BL.X);
                    command.Parameters.AddWithValue("@bl_y", ct.point_BL.Y);
                    command.Parameters.AddWithValue("@tl_x", ct.point_TL.X);
                    command.Parameters.AddWithValue("@tl_y", ct.point_TL.Y);
                    command.Parameters.AddWithValue("@tr_x", ct.point_TR.X);
                    command.Parameters.AddWithValue("@tr_y", ct.point_TR.Y);
                    command.Parameters.AddWithValue("@width", ct.frame_width);
                    command.Parameters.AddWithValue("@height", ct.frame_height);
                    command.Parameters.AddWithValue("@full_ct_filename", filename);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public static CoordinateTransformation LoadTransformation(int Id)
        {
            CoordinateTransformation ct = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Transformations WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double bl_x = Convert.ToDouble(reader["bl_x"]);
                            double bl_y = Convert.ToDouble(reader["bl_y"]);
                            double tl_x = Convert.ToDouble(reader["tl_x"]);
                            double tl_y = Convert.ToDouble(reader["tl_y"]);
                            double tr_x = Convert.ToDouble(reader["tr_x"]);
                            double tr_y = Convert.ToDouble(reader["tr_y"]);
                            PointF point_BL = new PointF((float)bl_x, (float)bl_y);
                            PointF point_TL = new PointF((float)tl_x, (float)tl_y);
                            PointF point_TR = new PointF((float)tr_x, (float)tr_y);
                            ct = new CoordinateTransformation(new PointF[] { point_BL, point_TL, point_TR });
                            ct.frame_width = Convert.ToInt32(reader["width"]);
                            ct.frame_height = Convert.ToInt32(reader["height"]);

                            string filename = Convert.ToString(reader["full_ct_filename"]);
                            if (filename != null && filename != "")
                                try { ct.FulCTFilename = filename; }
                                catch { }
                                
                        }
                    }
                }

            }
            return ct;

        }

        public static int SaveImageBatch(ImageBatch batch)
        {
            if (!(batch?.Count != 0)) return 0;    


            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string filenames_json = JsonSerializer.Serialize(batch.filenames);
                int ct_id = 0;
                int inner_id = ImageManager.GetIndex(batch);


                connection.Open();
                if (batch.coordinateTransformation != null)
                {
                    ct_id = SaveTransformation(batch.coordinateTransformation);
                }


                string sql = @"INSERT INTO ImageBatches
                     (InnerID, Transform_id, filenames_json)
                     VALUES (@InnerID, @Transform_id, @filenames_json); 
                    SELECT last_insert_rowid()";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@InnerID", inner_id);
                    command.Parameters.AddWithValue("@filenames_json", filenames_json);
                    if (ct_id != 0)
                        command.Parameters.AddWithValue("@Transform_id", ct_id);
                    else
                        command.Parameters.AddWithValue("@Transform_id", 0);

                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }



        }

        public static ImageBatch LoadImageBatch(int Id)
        {
             string[] labels = { "A", "B", "C" };
            
            ImageBatch imageBatch = new ImageBatch();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM ImageBatches WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            string flnms = Convert.ToString(reader["filenames_json"]);
                            List<string> filenames = JsonSerializer.Deserialize<List<string>>(flnms);
                            imageBatch.LocateImageBatch(filenames.ToArray());
                            int ct_id = Convert.ToInt32(reader["Transform_id"]);
                            if (ct_id != 0) imageBatch.coordinateTransformation = LoadTransformation(ct_id);
                            int name_id = Convert.ToInt32(reader["InnerID"]);
                            if (name_id >= 0 && name_id < 3) imageBatch.Name = labels[name_id];


                        }

                    }
                }

                return imageBatch;
            }
        }

        public static int SaveContainer(IContainer_2D container, int session_id)
        {
            string filename;

            if (container.Filename == "")
            {
                filename = "containers/container_" + DateTime.Now.Ticks + ".bin";
                container.SaveToFile(filename);
            }
            else
            {
                filename = container.Filename;
            }             

            int output_id = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"INSERT INTO Containers
                     (filename, image_group)
                     VALUES (@filename, @image_group); 
                    SELECT last_insert_rowid()";

                string sql2 = @"
                    INSERT INTO ContainersSessions
                    (session_id, container_id)
                    VALUES (@sid, @cid)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@filename", filename);
                    command.Parameters.AddWithValue("@image_group", container.ImageGroup);
                    output_id = Convert.ToInt32(command.ExecuteScalar());
                }

                using (SQLiteCommand command = new SQLiteCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@sid", session_id);
                    command.Parameters.AddWithValue("@cid", output_id);
                    output_id = Convert.ToInt32(command.ExecuteScalar());
                }

                return output_id;

            }



        }

        public static IContainer_2D LoadContainer(int Id)
        {
            IContainer_2D container = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM Containers WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string filename = Convert.ToString(reader["filename"]);
                            string image_group = Convert.ToString(reader["image_group"]);
                            container = Container_2D.ReadFromFile(filename);
                            container.ImageGroup = image_group;
                        }

                    }
                }

                return container;
            }



        }

        public static List<string> GetProfileNames()
        {
            List<string> names = new List<string>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM UserSessions";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read()) names.Add(Convert.ToString(reader["Name"]));
                    }
                }

            }

            return names;
        }

        public static void LoadXAxisSettings(int Id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT * FROM XAxisSettings WHERE Id = @Id";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string label = Convert.ToString(reader["Label"]);
                            string units = Convert.ToString(reader["Units"]);
                            double x_start = Convert.ToInt32(reader["Xstart"]);
                            double x_step = Convert.ToInt32(reader["Xstep"]);

                            DataManager_1D.Instance.VariableName = label;
                            DataManager_1D.Instance.VariableUnit = units;
                            DataManager_1D.Instance.x_start = x_start;
                            DataManager_1D.Instance.x_step = x_step;


                        }

                    }
                }

            }




        }

        public static int SaveXAxisSettings(int session_id)
        {
            int output_id = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sql = @"INSERT INTO XAxisSettings
                     (Label, Units, Xstart, Xstep)
                     VALUES (@Label, @Units, @Xstart, @Xstep);
                    SELECT last_insert_rowid()";

                string sql2 = @"
                    INSERT INTO AxisSessions
                    (session_id, xaxis_id)
                    VALUES (@sid, @xaxis_id)";

                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Label", DataManager_1D.Instance.VariableName);
                    command.Parameters.AddWithValue("@Units", DataManager_1D.Instance.VariableUnit);
                    command.Parameters.AddWithValue("@Xstart", DataManager_1D.Instance.x_start);
                    command.Parameters.AddWithValue("@Xstep", DataManager_1D.Instance.x_step);
                    output_id = Convert.ToInt32(command.ExecuteScalar());
                }

                using (SQLiteCommand command = new SQLiteCommand(sql2, connection))
                {
                    command.Parameters.AddWithValue("@sid", session_id);
                    command.Parameters.AddWithValue("@xaxis_id", output_id);
                    output_id = Convert.ToInt32(command.ExecuteScalar());
                }

                return output_id;



            }
        }

    }
}
