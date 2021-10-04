using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        void RegisterProduct(Shop shop, IEnumerable<string> productsName);
        Shop CreateShop(string shopName, string shopAddress);
        Shop CheapProductSearch(List<(Product, uint cheapCnt)> productsToBuyCheap);
        void AddProducts(Shop shop, List<(Product, ProductInfo)> productsToSetPrices);
        void PurchaseProduct(Customer customer, Shop shop, List<(Product, uint purchaseCnt)> productsToPurchase);
    }
}