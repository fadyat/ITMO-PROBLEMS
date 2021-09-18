using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        uint Id { get; }
        HashSet<Shop> CreatedShops { get; }
        HashSet<Product> RegisteredProducts { get; }
        Shop CreateShop(string shopName, string shopAddress);
        Product RegisterProduct(string productName);
        Shop CheapProductSearch(List<KeyValuePair<Product, ProductQuantity>> productsToBuyCheap);
        void AddProducts(Shop shop, List<KeyValuePair<Product, ProductQuantity>> productsToSetPrices, List<double> productPrices);
        void PurchaseProduct(Customer customer, Shop shop, List<KeyValuePair<Product, ProductQuantity>> productsToPurchase);
    }
}