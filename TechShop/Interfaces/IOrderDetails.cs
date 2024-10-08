using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Modals;

namespace TechShop.Interfaces
{
    public interface IOrderDetails
    {
        public decimal CalculateSubtotal(int orderDetailId);



        public void GetOrderDetailInfo(int orderDetailId);

    }
}