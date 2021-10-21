using System;
using System.Linq;
using Shops.Classes;
using Shops.Repositories;
using Shops.Services.Interfaces;

namespace Shops.Services
{
    public class ProductService : IProductService
    {
        private readonly OrderRepository _orderRepository;
        private readonly SupplyRepository _supplyRepository;
        private int _issuedProductId;
        private int _issuedOrderId;
        private int _issuedSupplyId;

        public ProductService()
        {
            ProductRepository = new ProductRepository();
            _orderRepository = new OrderRepository();
            _supplyRepository = new SupplyRepository();
            Registrations = new RegisteredProductRepository();
            _issuedProductId = 100000;
            _issuedOrderId = 100000;
            _issuedSupplyId = 100000;
        }

        public RegisteredProductRepository Registrations { get; }

        public ProductRepository ProductRepository { get; }

        public Product RegisterProduct(string name)
        {
            Product newProduct = new Product.ProductBuilder()
                .WithName(name)
                .WithId(_issuedProductId++)
                .Build();

            Registrations.Save(newProduct);
            return newProduct;
        }

        public void AddProduct(Shop shop, Product product, int price, int quantity)
        {
            Registrations.GetProduct(product.Id);
            var newSupply = new Supply(product, price, quantity, _issuedSupplyId++);
            _supplyRepository.Save(newSupply);

            Product recentProduct = ProductRepository.FindProduct(product.Id, shop.Id);
            int newQuantity = quantity;
            if (recentProduct != null)
            {
                newQuantity += recentProduct.Quantity;
                ProductRepository.Delete(product.Id, shop.Id);
            }

            Product newProduct = product.ToBuilder()
                .WithPrice(price)
                .WithShopId(shop.Id)
                .WithQuantity(newQuantity)
                .Build();

            ProductRepository.Save(newProduct);
        }

        public int PurchaseProduct(ref Customer customer, Shop shop, Product product, int amount)
        {
            Registrations.GetProduct(product.Id);
            var newOrder = new Order(product, amount, _issuedOrderId++);
            _orderRepository.Save(newOrder);

            Product recentProduct = ProductRepository.FindProduct(product.Id, shop.Id);
            if (recentProduct == null)
                throw new Exception(); // no such product!

            if (recentProduct.Quantity < amount)
                throw new Exception(); // not enough products!

            int expenses = recentProduct.Price * amount;
            if (customer.Cash < expenses)
                throw new Exception(); // not enough cash!

            ProductRepository.Delete(recentProduct.Id, recentProduct.ShopId);

            recentProduct = recentProduct.ToBuilder()
                .WithQuantity(recentProduct.Quantity - amount)
                .Build();

            ProductRepository.Save(recentProduct);
            customer = new Customer(customer.Cash - expenses);
            return expenses;
        }

        public int FindTheCheapestProductShopId(Product product, int amount)
        {
            int minimalPrice = (int)1e9;
            int cheapestShopId = 0;
            foreach (Product actual in ProductRepository.GetAll()
                .Where(actual => Equals(actual.Id, product.Id))
                .Where(actual => minimalPrice > actual.Price))
            {
                minimalPrice = actual.Price;
                cheapestShopId = actual.ShopId;
            }

            return cheapestShopId;
        }

        public void Print()
        {
            Console.WriteLine(" # Registered products:");
            foreach (Product product in Registrations.GetAll())
                Console.WriteLine($"\t {product}");

            Console.WriteLine(" # Products:");
            foreach (Product product in ProductRepository.GetAll())
                Console.WriteLine($"\t {product}");
        }
    }
}