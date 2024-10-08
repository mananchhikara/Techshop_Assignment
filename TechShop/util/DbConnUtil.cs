using System;
using System.Data.SqlClient;
using TechShop.Utility;


namespace TechShop.util
{
    public static class DBConnection
    {
        private static SqlConnection connection;

        public static SqlConnection GetConnection(string jsonFilePath)
        {
            if (connection == null)
            {
                try
                {
                    string connectionString = PropertyUtil.GetConnectionString(jsonFilePath);

                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        connection = new SqlConnection(connectionString);
                        connection.Open();
                        Console.WriteLine("Connection established successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to create connection. Invalid connection string.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error establishing connection: " + ex.Message);
                }
            }
            return connection;
        }

        public static void CloseConnection()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Connection closed successfully.");
            }
        }
    }
}