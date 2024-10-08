using System;
using TechShop.Services;

namespace TechShop.Main
{
    public class Menu
    {
        public void DisplayMenu()
        {
            bool continueRunning = true;

            while (continueRunning)
            {
                Console.Clear();
                Console.WriteLine("TechShop!");
                Console.WriteLine("1. View All Customers");
                Console.WriteLine("2. View Customer Details");
                Console.WriteLine("3. Update Customer Information");
                Console.WriteLine("4. Calculate Total Orders for a Customer");
                Console.WriteLine("5. View Product Details");
                Console.WriteLine("6. Update Product Information");
                Console.WriteLine("7. Check Product Stock");
                Console.WriteLine("8. Calculate Total Order Amount");
                Console.WriteLine("9. Update Order Status");
                Console.WriteLine("10. Cancel Order");
                Console.WriteLine("11. Exit");
                Console.Write("Please select an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ViewAllCustomers();
                        break;
                    case "2":
                        ViewCustomerDetails();
                        break;
                    case "3":
                        UpdateCustomerInformation();
                        break;
                    case "4":
                        CalculateTotalOrdersForCustomer();
                        break;
                    case "5":
                        ViewProductDetails();
                        break;
                    case "6":
                        UpdateProductInformation();
                        break;
                    case "7":
                        CheckProductStock();
                        break;
                    case "8":
                        CalculateTotalOrderAmount();
                        break;
                    case "9":
                        UpdateOrderStatus();
                        break;
                    case "10":
                        CancelOrder();
                        break;
                    case "11":
                        continueRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }

                if (continueRunning)
                {
                    Console.WriteLine("Press any key to return to the main menu...");
                    Console.ReadKey();
                }
            }
        }

        static void ViewAllCustomers()
        {
            CustomerService customerService = new CustomerService();
            var customers = customerService.GetAllCusto(); // Corrected method name
            Console.WriteLine("Customers:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"ID {customer.CustomerID},Name {customer.FirstName} {customer.LastName}, Email:{customer.Email}");
            }
        }

        static void ViewCustomerDetails()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            CustomerService customerService = new CustomerService();
            var customer = customerService.GetCustomerDetails(customerId);

            if (customer != null)
            {
                Console.WriteLine($"ID: {customer.CustomerID}");
                Console.WriteLine($"Name: {customer.FirstName} {customer.LastName}");
                Console.WriteLine($"Email: {customer.Email}");
                Console.WriteLine($"Phone: {customer.Phone}");
                Console.WriteLine($"Address: {customer.Address}");
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static void UpdateCustomerInformation()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            CustomerService customerService = new CustomerService();
            var customer = customerService.GetCustomerDetails(customerId);

            if (customer != null)
            {
                Console.Write("Enter new first name: ");
                customer.FirstName = Console.ReadLine();
                Console.Write("Enter new last name: ");
                customer.LastName = Console.ReadLine();
                Console.Write("Enter new email: ");
                customer.Email = Console.ReadLine();
                Console.Write("Enter new phone: ");
                customer.Phone = Console.ReadLine();
                Console.Write("Enter new address: ");
                customer.Address = Console.ReadLine();

                bool isUpdated = customerService.UpdateCustomerInfo(customer);
                if (isUpdated)
                {
                    Console.WriteLine("Customer information updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update customer information.");
                }
            }
            else
            {
                Console.WriteLine("Customer not found.");
            }
        }

        static void CalculateTotalOrdersForCustomer()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            CustomerService customerService = new CustomerService();
            int totalOrders = customerService.CalculateTotalOrders(customerId);
            Console.WriteLine($"Total Orders for Customer {customerId}: {totalOrders}");
        }

        static void ViewProductDetails()
        {
            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            ProductService productService = new ProductService();
            var product = productService.GetProductDetails(productId);

            if (product != null)
            {
                Console.WriteLine($"ID: {product.ProductID}");
                Console.WriteLine($"Name: {product.ProductName}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine($"Description: {product.Description}");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        static void UpdateProductInformation()
        {
            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            ProductService productService = new ProductService();
            var product = productService.GetProductDetails(productId);

            if (product != null)
            {
                Console.Write("Enter new product name: ");
                product.ProductName = Console.ReadLine();
                Console.Write("Enter new price: ");
                product.Price = decimal.Parse(Console.ReadLine());
                Console.Write("Enter new description: ");
                product.Description = Console.ReadLine();

                bool isUpdated = productService.UpdateProductInfo(product);
                if (isUpdated)
                {
                    Console.WriteLine("Product information updated successfully.");
                }
                else
                {
                    Console.WriteLine("Failed to update product information.");
                }
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        static void CheckProductStock()
        {
            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());

            ProductService productService = new ProductService();
            bool inStock = productService.IsProductInStock(productId);

            if (inStock)
            {
                Console.WriteLine("Product is in stock.");
            }
            else
            {
                Console.WriteLine("Product is out of stock.");
            }
        }

        static void CalculateTotalOrderAmount()
        {
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            OrderService orderService = new OrderService();
            decimal totalAmount = orderService.CalculateTotalAmount(orderId);
            Console.WriteLine($"Total Amount for Order {orderId}: {totalAmount:C}");
        }

        static void UpdateOrderStatus()
        {
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            Console.Write("Enter new order status: ");
            string status = Console.ReadLine();

            OrderService orderService = new OrderService();
            bool isUpdated = orderService.UpdateOrderStatus(orderId, status);

            if (isUpdated)
            {
                Console.WriteLine("Order status updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update order status.");
            }
        }

        static void CancelOrder()
        {
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            OrderService orderService = new OrderService();
            bool isCancelled = orderService.CancelOrder(orderId);

            if (isCancelled)
            {
                Console.WriteLine("Order cancelled successfully.");
            }
            else
            {
                Console.WriteLine("Failed to cancel the order.");
            }
        }
    }
}