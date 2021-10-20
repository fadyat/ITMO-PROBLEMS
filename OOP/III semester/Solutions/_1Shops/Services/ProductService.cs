using System.Collections.Generic;
using Shops.Classes;
using Shops.Interfaces;

namespace Shops.Services
{
    public class ProductService : IProductService
    {
        private List<Product> _products;
        private OrderService _orders;
        private SupplyService _supplies;
        private int _issuedId;

        public ProductService()
        {
            _products = new List<Product>();
            _orders = new OrderService();
            _supplies = new SupplyService();
            _issuedId = 100000;
        }

        public Product RegisterProduct(string name)
        {
            Product newProduct = new Product.ProductBuilder()
                .WithName(name)
                .WithId(_issuedId++)
                .Build();

            _products.Add(newProduct);
            return newProduct;
        }

        public void AddProduct(Shop shop, Product product, int amount, int quantity)
        {
            _supplies = new SupplyService()
                .ToBuilder()
                .AddSupply(product, amount, quantity)
                .Build();

            // ...
        }

        public void PurchaseProduct(Customer customer, Shop shop, Product product, int amount)
        {
            _orders = new OrderService()
                .Build()
                .AddOrder(product, amount)
                .Build();

            // ...
        }

        public Product FindProduct(Shop shop, Product product)
        {
            // ...
        }
    }
}