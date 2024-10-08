using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Modals
{
    public class Product
    {
        private decimal price;

        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }

        
        public decimal Price
        {
            get { return price; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Price cannot be negative.");
                }
                price = value;
            }
        }

        public Product(int productId, string productName, string description, decimal price)
        {
            ProductID = productId;
            ProductName = productName;
            Description = description;
            Price = price; 
        }

       
        public Product() { }
    }

}
