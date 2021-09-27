using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        Shop CreateShop(string shopName, string shopAddress);
        Product RegisterProduct(string productName);
        Shop CheapProductSearch(List<(Product, uint need)> productsToBuyCheap);
        void AddProducts(Shop shop, List<(Product, uint have, double price)> products);
        void PurchaseProduct(Customer customer, Shop shop, List<(Product, uint need)> productsToPurchase);
    }
}