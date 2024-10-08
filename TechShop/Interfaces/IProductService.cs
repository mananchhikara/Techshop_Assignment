using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Modals;

namespace TechShop.Interfaces
{
    public interface IProductService
    {
        Product GetProductDetails(int productid);
        bool UpdateProductInfo(Product product);
        bool IsProductInStock(int productid);
    }
}