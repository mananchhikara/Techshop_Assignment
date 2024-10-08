
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
    public class InventoryService : IInventoryService
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;
        string filepath = Config.DbConfigFilePath;
        public InventoryService()
        {
            sqlConnection = DBConnection.GetConnection(filepath);
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
        }

        public Product GetProduct(int productId)
        {
            Product product = null;
            try
            {
                cmd.CommandText = "SELECT * FROM Products WHERE ProductID = @ProductID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductID", productId);

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["Name"].ToString(),
                        Price = (decimal)reader["Price"]
                    };
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving the product: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return product;
        }

        // GetQuantityInStock: Gets the current quantity of the product in stock
        public int GetQuantityInStock(int productId)
        {
            int quantity = 0;

            try
            {
                cmd.CommandText = "SELECT Quantity FROM Inventory WHERE ProductID = @ProductID";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductID", productId);

                sqlConnection.Open();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    quantity = (int)result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while getting quantity in stock: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return quantity;
        }

        // AddToInventory: Adds a specified quantity of the product to the inventory
        public void AddToInventory(int productId, int quantityToAdd)
        {
            if (quantityToAdd > 0)
            {
                try
                {
                    cmd.CommandText = "UPDATE Inventory SET Quantity = Quantity + @Quantity WHERE ProductID = @ProductID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Quantity", quantityToAdd);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Added " + quantityToAdd + " to inventory.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to add to inventory. Check if ProductID exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while adding to inventory: " + ex.Message);
                }
                finally
                {
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                }
            }
            else
            {
                Console.WriteLine("Quantity to add must be a positive integer.");
            }
        }

        // RemoveFromInventory: Removes a specified quantity of the product from the inventory
        public void RemoveFromInventory(int productId, int quantityToRemove)
        {
            if (quantityToRemove > 0)
            {
                int currentQuantity = GetQuantityInStock(productId);
                if (currentQuantity >= quantityToRemove)
                {
                    try
                    {
                        cmd.CommandText = "UPDATE Inventory SET Quantity = Quantity - @Quantity WHERE ProductID = @ProductID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@Quantity", quantityToRemove);
                        cmd.Parameters.AddWithValue("@ProductID", productId);

                        sqlConnection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Removed " + quantityToRemove + " from inventory.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to remove from inventory. Check if ProductID exists.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while removing from inventory: " + ex.Message);
                    }
                    finally
                    {
                        if (sqlConnection.State == System.Data.ConnectionState.Open)
                        {
                            sqlConnection.Close();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Insufficient stock to remove " + quantityToRemove + ". Current stock: " + currentQuantity);
                }
            }
            else
            {
                Console.WriteLine("Quantity to remove must be a positive integer.");
            }
        }

        // UpdateStockQuantity: Updates the stock quantity to a new value
        public void UpdateStockQuantity(int productId, int newQuantity)
        {
            if (newQuantity >= 0) // Ensure quantity is non-negative
            {
                try
                {
                    cmd.CommandText = "UPDATE Inventory SET Quantity = @Quantity WHERE ProductID = @ProductID";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Stock quantity updated to " + newQuantity + ".");
                    }
                    else
                    {
                        Console.WriteLine("Failed to update stock quantity. Check if ProductID exists.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while updating stock quantity: " + ex.Message);
                }
                finally
                {
                    if (sqlConnection.State == System.Data.ConnectionState.Open)
                    {
                        sqlConnection.Close();
                    }
                }
            }
            else
            {
                Console.WriteLine("New quantity must be a non-negative integer.");
            }
        }

        // IsProductAvailable: Checks if a specified quantity of the product is available in the inventory
        public bool IsProductAvailable(int productId, int quantityToCheck)
        {
            int currentQuantity = GetQuantityInStock(productId);
            return currentQuantity >= quantityToCheck;
        }

        // GetInventoryValue: Calculates the total value of the products in the inventory
        public decimal GetInventoryValue()
        {
            decimal totalValue = 0;

            try
            {
                cmd.CommandText = "SELECT SUM(p.Price * i.Quantity) FROM Products p JOIN Inventory i ON p.ProductID = i.ProductID";
                sqlConnection.Open();
                var result = cmd.ExecuteScalar();

                if (result != null)
                {
                    totalValue = (decimal)result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while calculating inventory value: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }

            return totalValue;
        }

        // ListLowStockProducts: Lists products with quantities below a specified threshold
        public void ListLowStockProducts(int threshold)
        {
            try
            {
                cmd.CommandText = "SELECT p.Name, i.Quantity FROM Products p JOIN Inventory i ON p.ProductID = i.ProductID WHERE i.Quantity < @Threshold";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Threshold", threshold);

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("Low Stock Products:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product Name: {reader["Name"]}, Quantity: {reader["Quantity"]}");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while listing low stock products: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        // ListOutOfStockProducts: Lists products that are out of stock
        public void ListOutOfStockProducts()
        {
            try
            {
                cmd.CommandText = "SELECT p.Name FROM Products p JOIN Inventory i ON p.ProductID = i.ProductID WHERE i.Quantity = 0";

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("Out of Stock Products:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product Name: {reader["Name"]}");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while listing out of stock products: " + ex.Message);
            }
            finally
            {
                if (sqlConnection.State == System.Data.ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }

        // ListAllProducts: Lists all products in the inventory, along with their quantities
        public void ListAllProducts()
        {
            try
            {
                cmd.CommandText = "SELECT p.Name, i.Quantity FROM Products p JOIN Inventory i ON p.ProductID = i.ProductID";

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                Console.WriteLine("All Products in Inventory:");
                while (reader.Read())
                {
                    Console.WriteLine($"Product Name: {reader["Name"]}, Quantity: {reader["Quantity"]}");
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while listing all products: " + ex.Message);
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
