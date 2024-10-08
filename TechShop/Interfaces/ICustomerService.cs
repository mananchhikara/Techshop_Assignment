using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Modals;

namespace TechShop.Interfaces
{
    public interface ICustomerService
    {
        int CalculateTotalOrders(int customerid);
        bool UpdateCustomerInfo(Customer customer);
        Customer GetCustomerDetails(int customerid);


    }
}