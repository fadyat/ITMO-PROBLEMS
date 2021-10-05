using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShop
    {
        void RegisterProduct(IEnumerable<string> productsName);
        void AddProducts(List<(Product, ProductInfo)> products);
        void PurchaseProduct(ref Customer customer, List<(Product, uint purchaseCnt)> productsToPurchase);
    }
}