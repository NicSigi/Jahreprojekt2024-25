using Microsoft.Data.SqlClient;
using StudioManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace JahresprojektNeu
{
    internal class SQLManagement : BCrypt
    {
        private static SqlConnection con = new SqlConnection();
        private static SqlCommand cmd = new SqlCommand("", con);
        private static SqlDataReader SQLReader = null;
        private static string Connection;
        private static string DatabaseName = "OnlineCasino";
        private static string[] TableNames = { "Users" };
        private static Dictionary<string, string> tableCommands = new Dictionary<string, string>
        {
            {
                "Users",
                "Create Table Users(" +
                "UserID int identity(1,1) primary key," +
                "Username varchar(255) not null," +
                "HashPass varchar(max) not null);"
            }
        };
        private static SqlDataAdapter SQLDA = new SqlDataAdapter("", con);
        private static DataTable dataTable = new DataTable();

        public static void Autoconnect()
        {
            Connection = "server=(localdb)\\MSSQLLocalDB;integrated Security=true;";
            con = new SqlConnection(Connection);
            try
            {
                con.Open();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            LoadOrCreateDatabase();
            LoadOrCreateTables();
            CreateDefaultAdmin();
        }

        /// <summary>
        /// Checks if Database exists
        /// if not creates it
        /// and updates connection string
        /// </summary>
        public static void LoadOrCreateDatabase()
        {
            try
            {
                con.Open();
                string CheckIfExistsQuery = $"SELECT name FROM sys.databases where name='{DatabaseName}'";
                cmd = new SqlCommand(CheckIfExistsQuery, con);
                SQLReader = cmd.ExecuteReader();
                if (SQLReader.HasRows)
                {
                    SQLReader.Close();
                    con.Close();
                }
                else
                {
                    SQLReader.Close();
                    string createDatabaseQuery = $"CREATE DATABASE [{DatabaseName}];";
                    cmd.CommandText = createDatabaseQuery;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                Connection = $"server=(localdb)\\MSSQLLocalDB;Database={DatabaseName};integrated Security=true;";
                con.ConnectionString = Connection;
                cmd = new SqlCommand("", con);
                SQLDA = new SqlDataAdapter("", con);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void LoadOrCreateTables()
        {
            try
            {
                con.Open();
                foreach (string tableName in TableNames)
                {
                    string CheckIfTableExistsQuery = $"SELECT name from sys.tables where name='{tableName}';";
                    cmd.CommandText = CheckIfTableExistsQuery;
                    SQLReader = cmd.ExecuteReader();
                    if (SQLReader.HasRows)
                    {
                        SQLReader.Close();
                    }
                    else
                    {
                        SQLReader.Close();
                        string CreateTableBooksQuery = tableCommands[tableName];
                        cmd.CommandText = CreateTableBooksQuery;
                        MessageBox.Show(cmd.CommandText.ToString());
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Created a default admin user with username "admin" and password "admin"
        /// </summary>
        public static void CreateDefaultAdmin()
        {
            try
            {
                string username = "admin";
                string password = "admin";
                string Exists = $"SELECT Username FROM Users;";
                string salt = BCrypt.GenerateSalt(12);
                string hashedPass = BCrypt.HashPassword(password, salt);

                cmd.CommandText = Exists;
                con.Open();

                if (cmd.ExecuteScalar() == null)
                {
                    string addAdmin = $"INSERT INTO Users(Username, HashPass) VALUES('{username}', '{hashedPass}')";
                    cmd.CommandText = addAdmin;
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Ihr Username: 'admin' \n" +
                                    "Ihr Passwort: 'admin' \n" +
                                    "Merken Sie sich diese Anmeldedaten!\n" +
                                    "Später können Sie noch weitere User erstellen bzw. das Passwort dieses Users ändern!",
                                    "Wichtige Anmeldeinformationen",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        /// <summary>
        /// Authenticates a user by checking username and password against the database
        /// </summary>
        /// <param name="username">Username to check</param>
        /// <param name="password">Password to verify</param>
        /// <returns>True if authentication is successful, false otherwise</returns>
        public static bool AuthenticateUser(string username, string password)
        {
            bool authenticated = false;

            try
            {
                con.Open();
                string query = $"SELECT HashPass FROM Users WHERE Username = @Username";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                SQLReader = cmd.ExecuteReader();

                if (SQLReader.HasRows)
                {
                    SQLReader.Read();
                    string storedHash = SQLReader["HashPass"].ToString();

                    // Verify the password against the stored hash
                    authenticated = BCrypt.CheckPassword(password, storedHash);
                }

                SQLReader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return authenticated;
        }

        /// <summary>
        /// Registers a new user in the database
        /// </summary>
        /// <param name="username">Username for the new user</param>
        /// <param name="password">Password for the new user</param>
        /// <returns>True if registration is successful, false otherwise</returns>
        public static bool RegisterUser(string username, string password)
        {
            bool success = false;

            try
            {
                // Check if username already exists
                con.Open();
                string checkQuery = $"SELECT COUNT(*) FROM Users WHERE Username = @Username";
                cmd = new SqlCommand(checkQuery, con);
                cmd.Parameters.AddWithValue("@Username", username);

                int userCount = (int)cmd.ExecuteScalar();

                if (userCount > 0)
                {
                    con.Close();
                    return false; // Username already exists
                }

                // Hash the password
                string salt = BCrypt.GenerateSalt(12);
                string hashedPass = BCrypt.HashPassword(password, salt);

                // Insert new user
                string insertQuery = $"INSERT INTO Users(Username, HashPass) VALUES(@Username, @HashPass)";
                cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@HashPass", hashedPass);

                int rowsAffected = cmd.ExecuteNonQuery();
                success = (rowsAffected > 0);

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return success;
        }
    }
}