
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechShop.Modals
{
    public class OrderDetails
    {
        private int quantity;

        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        // Quantity with validation
        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Quantity must be a positive integer.");
                }
                quantity = value;
            }
        }

        public OrderDetails(int orderDetailId, int orderid, int productid, int quantity)
        {
            OrderDetailID = orderDetailId;
            OrderID = orderid;
            ProductID = productid;
            Quantity = quantity; // Will go through validation
        }

        public OrderDetails() { }
    }
}
