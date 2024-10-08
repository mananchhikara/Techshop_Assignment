using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechShop.Modals;

namespace TechShop.Interfaces
{
    public interface IInventoryService
    {
        public Product GetProduct(int productId);

        public int GetQuantityInStock(int productId);

        public void AddToInventory(int productId, int quantityToAdd);

        public void RemoveFromInventory(int productId, int quantityToRemove);
        public void UpdateStockQuantity(int productId, int newQuantity);

        public bool IsProductAvailable(int productId, int quantityToCheck);
        public decimal GetInventoryValue();
        public void ListLowStockProducts(int threshold);

        public void ListOutOfStockProducts();

        public void ListAllProducts();

    }
}