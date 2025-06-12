using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace JahresprojektNeu.Classes
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
                "HashPass varchar(max) not null," +
                "Balance decimal(18, 2) not null default(100)" +
                ");"
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
                        string CreateTableQuery = tableCommands[tableName];
                        cmd.CommandText = CreateTableQuery;
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
        /// Creates a default admin user with username "admin" and password "admin" with initial balance 0
        /// </summary>
        public static void CreateDefaultAdmin()
        {
            try
            {
                string username = "admin";
                string password = "admin";

                string Exists = $"SELECT Username FROM Users WHERE Username = @Username;";
                cmd = new SqlCommand(Exists, con);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();

                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    string salt = GenerateSalt(12);
                    string hashedPass = HashPassword(password, salt);

                    string addAdmin = $"INSERT INTO Users(Username, HashPass, Balance) VALUES(@Username, @HashPass, @Balance)";
                    cmd = new SqlCommand(addAdmin, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@HashPass", hashedPass);
                    cmd.Parameters.AddWithValue("@Balance", 0m);

                    cmd.ExecuteNonQuery();

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
                if (con.State == ConnectionState.Open) con.Close();
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
                    authenticated = CheckPassword(password, storedHash);
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
                string salt = GenerateSalt(12);
                string hashedPass = HashPassword(password, salt);

                // Insert new user with Balance 0 as initial value
                string insertQuery = $"INSERT INTO Users(Username, HashPass, Balance) VALUES(@Username, @HashPass, @Balance)";
                cmd = new SqlCommand(insertQuery, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@HashPass", hashedPass);
                cmd.Parameters.AddWithValue("@Balance", 0m);

                int rowsAffected = cmd.ExecuteNonQuery();
                success = rowsAffected > 0;

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

        /// <summary>
        /// Gets the balance of a user by username.
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <returns>The balance of the user</returns>
        public static decimal GetUserBalance(string username)
        {
            decimal balance = 0;

            try
            {
                con.Open();
                string query = $"SELECT Balance FROM Users WHERE Username = @Username";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    balance = Convert.ToDecimal(result);
                }

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

            return balance;
        }

        /// <summary>
        /// Updates the balance of a user by username.
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="newBalance">New balance to set</param>
        /// <returns>True if the update is successful, false otherwise</returns>
        public static bool UpdateUserBalance(string username, decimal newBalance)
        {
            bool success = false;

            try
            {
                con.Open();
                string query = $"UPDATE Users SET Balance = @Balance WHERE Username = @Username";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Balance", newBalance);
                cmd.Parameters.AddWithValue("@Username", username);

                int rowsAffected = cmd.ExecuteNonQuery();
                success = rowsAffected > 0;

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

