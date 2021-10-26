using System.Collections.Generic;
using Shops.Classes;
using Shops.Repositories.Interfaces;

namespace Shops.Services.Interfaces
{
    public interface IProductService
    {
        IProductRepository ProductRepository { get; }

        ISupplyRepository SupplyRepository { get; }

        IOrderRepository OrderRepository { get; }

        Product RegisterProduct(string name);

        void AddProduct(Shop shop, Product product, int price, int quantity);

        void AddProducts(Shop shop, List<Product> products, List<int> prices, List<int> quantities);

        int PurchaseProduct(ref Customer customer, Shop shop, Product product, int amount);

        int PurchaseProducts(ref Customer customer, Shop shop, List<Product> products, List<int> amounts);
    }
}