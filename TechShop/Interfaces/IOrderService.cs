using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Modals;

namespace TechShop.Interfaces
{
    public interface IOrderService
    {
        decimal CalculateTotalAmount(int orderid);

        public List<OrderDetails> GetOrderDetails(int orderId);

        public bool UpdateOrderStatus(int orderId, string status);
        public bool CancelOrder(int orderId);
    }
}