using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Interfaces;
using TechShop.Modals;
using TechShop.util;

namespace TechShop.Services
{
    public class OrderService : IOrderService
    {

        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;
        string filepath = Config.DbConfigFilePath;
        public OrderService()
        {
            sqlConnection = DBConnection.GetConnection(filepath);
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
        }


        public decimal CalculateTotalAmount(int orderId)
        {
            decimal totalAmount = 0;

            try
            {
                cmd.CommandText = @"SELECT od.Quantity, p.Price 
                            FROM OrderDetails od 
                            JOIN Products p ON od.ProductID = p.ProductID 
                            WHERE od.OrderID = @OrderID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int quantity = (int)reader["Quantity"];
                    decimal price = (decimal)reader["Price"];
                    totalAmount += quantity * price;
                }

                reader.Close();

                // Update total amount in the Orders table
                cmd.CommandText = "UPDATE Orders SET TotalAmount = @TotalAmount WHERE OrderID = @OrderID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while calculating total order amount: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return totalAmount;
        }


        public List<OrderDetails> GetOrderDetails(int orderId)
        {
            List<OrderDetails> orderDetails = new List<OrderDetails>();

            try
            {
                cmd.CommandText = @"SELECT * FROM OrderDetails WHERE OrderID = @OrderID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderID", orderId);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    OrderDetails detail = new OrderDetails
                    {
                        OrderID = (int)reader["OrderID"],
                        ProductID = (int)reader["ProductID"],
                        Quantity = (int)reader["Quantity"],
                        OrderDetailID = (int)reader["OrderDetailID"]
                    };

                    orderDetails.Add(detail);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving order details: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return orderDetails;
        }





        public bool UpdateOrderStatus(int orderId, string status)
        {
            bool isUpdated = false;

            try
            {
                cmd.CommandText = "UPDATE Orders SET Status = @OrderStatus WHERE OrderID = @OrderID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrderID", orderId);
                cmd.Parameters.AddWithValue("@OrderStatus", status);

                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection.Open();
                }

                int rowsAffected = cmd.ExecuteNonQuery();
                isUpdated = rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while updating order status: " + ex.Message);
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


        public bool CancelOrder(int orderId)
        {
            bool isCancelled = false;

            try
            {
                // Begin transaction to ensure atomicity of cancellation and stock adjustment
                sqlConnection.Open();
                SqlTransaction transaction = sqlConnection.BeginTransaction();
                cmd.Transaction = transaction;

                try
                {
                    // Get the order details (products and quantities) to adjust stock
                    cmd.CommandText = @"SELECT ProductID, Quantity FROM OrderDetails WHERE OrderID = @OrderID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@OrderID", orderId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, int> productQuantities = new Dictionary<int, int>();

                    while (reader.Read())
                    {
                        int productId = (int)reader["ProductID"];
                        int quantity = (int)reader["Quantity"];
                        productQuantities.Add(productId, quantity);
                    }

                    reader.Close();

                    // Update stock levels in the Inventory table
                    foreach (var entry in productQuantities)
                    {
                        cmd.CommandText = @"UPDATE Inventory 
                                    SET QuantityInStock = QuantityInStock + @Quantity 
                                    WHERE ProductID = @ProductID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Quantity", entry.Value);
                        cmd.Parameters.AddWithValue("@ProductID", entry.Key);

                        cmd.ExecuteNonQuery();
                    }

                    // Update order status to 'Cancelled'
                    cmd.CommandText = "UPDATE Orders SET Status = 'Cancelled' WHERE OrderID = @OrderID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@OrderID", orderId);
                    cmd.ExecuteNonQuery();

                    // Commit the transaction
                    transaction.Commit();
                    isCancelled = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("An error occurred while cancelling the order: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while processing the cancellation: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return isCancelled;
        }






    }
}