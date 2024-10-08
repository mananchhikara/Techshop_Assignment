
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Interfaces;
using TechShop.Modals;
using TechShop.util;


namespace TechShop.Services
{
    public class CustomerService : ICustomerService
    {

        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;
        string filepath = Config.DbConfigFilePath;
        public CustomerService()
        {
            sqlConnection = DBConnection.GetConnection(filepath);
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
        }


        // Method to calculate total orders placed by a customer


        public List<Customer> GetAllCusto()
        {
            List<Customer> customers = new List<Customer>();
            try
            {
                // Open the connection
                //sqlConnection.Open();

                // Execute the query
                cmd.CommandText = "SELECT * FROM Customers";
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }
                SqlDataReader reader = cmd.ExecuteReader();

                // Process the results
                while (reader.Read())
                {
                    Customer customer1 = new Customer
                    {
                        CustomerID = (int)reader["CustomerID"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Email = (string)reader["Email"],
                        Phone = (string)reader["Phone"],
                        Address = (string)reader["Address"]
                    };

                    customers.Add(customer1);
                }

                // Close the reader after processing
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is closed
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return customers;
        }


        public int CalculateTotalOrders(int customerId)
        {
            int totalOrders = 0;
            try
            {
                cmd.CommandText = "SELECT COUNT(*) FROM Orders WHERE CustomerID = @CustomerID";
                cmd.Parameters.Clear();  // Clear any previous parameters
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                totalOrders = (int)cmd.ExecuteScalar();  // Use ExecuteScalar to get a single value (count)
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while calculating total orders: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return totalOrders;
        }

        public Customer GetCustomerDetails(int customerId)
        {
            Customer customer = null;

            try
            {
                cmd.CommandText = "SELECT * FROM Customers WHERE CustomerID = @CustomerID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CustomerID", customerId);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    customer = new Customer
                    {
                        CustomerID = (int)reader["CustomerID"],
                        FirstName = (string)reader["FirstName"],
                        LastName = (string)reader["LastName"],
                        Email = (string)reader["Email"],
                        Phone = (string)reader["Phone"],
                        Address = (string)reader["Address"]
                    };
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving customer details: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return customer;
        }

        public bool UpdateCustomerInfo(Customer customer)
        {
            bool isUpdated = false;

            try
            {
                cmd.CommandText = @"UPDATE Customers 
                            SET FirstName = @FirstName, 
                                LastName = @LastName, 
                                Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address 
                            WHERE CustomerID = @CustomerID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CustomerID", customer.CustomerID);
                cmd.Parameters.AddWithValue("@FirstName", customer.FirstName);
                cmd.Parameters.AddWithValue("@LastName", customer.LastName);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Phone", customer.Phone);
                cmd.Parameters.AddWithValue("@Address", customer.Address);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating customer info: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return isUpdated;
        }






    }
}
