using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        Shop CreateShop(string shopName);

        Product RegisterProduct(string productNameForRegistration);

        List<Product> RegisterProducts(IEnumerable<string> productNamesForRegistration);

        StoredProducts AddProducts(Shop shop, List<Product> productToAdd, List<ProductInfo> productInfo);

        uint PurchaseProducts(ref Customer customer, Shop shop, List<Product> productToPurchase, List<uint> quantities);

        Shop FindCheapestShop(List<Product> productsToBuyCheap, List<uint> quantities);
    }
}