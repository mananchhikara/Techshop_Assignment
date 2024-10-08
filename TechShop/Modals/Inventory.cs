using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Modals
{
    public class Inventory
    {
        private int quantityInStock;

        public int InventoryID { get; set; }
        public Product Product { get; set; }
        public DateTime LastStockUpdate { get; set; }

        
        public int QuantityInStock
        {
            get { return quantityInStock; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Quantity in stock cannot be negative.");
                }
                quantityInStock = value;
            }
        }


        public static List<Inventory> InventoryItems { get; set; } = new List<Inventory>();

        public Inventory(int inventoryID, Product product, int quantityInStock, DateTime lastStockUpdate)
        {
            InventoryID = inventoryID;
            Product = product;
            QuantityInStock = quantityInStock;
            LastStockUpdate = lastStockUpdate;
        }

        
        public Inventory() { }






    }
}
