using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Modals;
using TechShop.Interfaces;
using System.Data.SqlClient;
using TechShop.util;

namespace TechShop.Services
{
    public class OrderDetailsService : IOrderDetails
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;
        string filepath = Config.DbConfigFilePath;
        public OrderDetailsService()
        {
            sqlConnection = DBConnection.GetConnection(filepath);
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
        }


        public decimal CalculateSubtotal(int orderDetailId)
        {
            decimal subtotal = 0;

            try
            {
                // Query to get the product price and quantity from the OrderDetails table
                cmd.CommandText = "SELECT od.Quantity, p.Price " +
                                  "FROM OrderDetails od " +
                                  "JOIN Products p ON od.ProductID = p.ProductID " +
                                  "WHERE od.OrderDetailID = @OrderDetailID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetailId);

                // Open the connection
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Get the quantity and price from the database
                        int quantity = (int)reader["Quantity"];
                        decimal price = (decimal)reader["Price"];

                        // Calculate subtotal
                        subtotal = quantity * price;
                    }
                    else
                    {
                        Console.WriteLine($"No record found for OrderDetailID {orderDetailId}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while calculating subtotal: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is closed
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return subtotal;
        }




        public void GetOrderDetailInfo(int orderDetailId)
        {
            try
            {
                // Query to retrieve order detail info along with product name and price
                cmd.CommandText = @"
            SELECT od.OrderDetailID, od.OrderID, od.ProductID, od.Quantity, 
                   p.ProductName, p.Price 
            FROM OrderDetails od
            JOIN Products p ON od.ProductID = p.ProductID
            WHERE od.OrderDetailID = @OrderDetailID";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderDetailID", orderDetailId);

                // Open the connection
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Order Detail ID: " + reader["OrderDetailID"]);
                        Console.WriteLine("Order ID: " + reader["OrderID"]);
                        Console.WriteLine("Product ID: " + reader["ProductID"]);
                        Console.WriteLine("Product Name: " + reader["ProductName"]);
                        Console.WriteLine("Quantity: " + reader["Quantity"]);
                        Console.WriteLine("Price: " + ((decimal)reader["Price"]).ToString("C")); // Format as currency
                        //Console.WriteLine("Subtotal: " + CalculateSubtotal(orderDetailId).ToString("C")); // Calculate and format subtotal as currency
                    }
                    else
                    {
                        Console.WriteLine("No details found for this Order Detail ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }










    }
}