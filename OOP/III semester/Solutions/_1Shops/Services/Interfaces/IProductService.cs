using Shops.Classes;

namespace Shops.Services.Interfaces
{
    public interface IProductService
    {
        Product RegisterProduct(string name);

        void AddProduct(Shop shop, Product product, int amount, int price);

        int PurchaseProduct(ref Customer customer, Shop shop, Product product, int amount);

        int FindTheCheapestProductShopId(Product product, int amount);
    }
}