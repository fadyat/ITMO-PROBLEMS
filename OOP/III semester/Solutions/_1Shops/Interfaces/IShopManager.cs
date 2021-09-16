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
        Shop CheapProductSearch(ref List<KeyValuePair<Product, ProductQuantity>> productsToBuyCheap);
        void AddProducts(ref Shop shop, ref List<KeyValuePair<Product, ProductQuantity>> productsToSetPrices, ref List<double> productPrices);
        void PurchaseProduct(Customer customer, Shop shop, List<KeyValuePair<Product, ProductQuantity>> productsToPurchase);
    }
}