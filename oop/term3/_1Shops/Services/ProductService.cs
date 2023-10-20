using System.Collections.Generic;
using Shops.Classes;
using Shops.Exceptions;
using Shops.Repositories.Interfaces;
using Shops.Services.Interfaces;

namespace Shops.Services
{
    public class ProductService : IProductService
    {
        private int _issuedProductId;
        private int _issuedOrderId;
        private int _issuedSupplyId;

        public ProductService(
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            ISupplyRepository supplyRepository)
        {
            ProductRepository = productRepository;
            OrderRepository = orderRepository;
            SupplyRepository = supplyRepository;
            _issuedProductId = 100000;
            _issuedOrderId = 100000;
            _issuedSupplyId = 100000;
        }

        public IProductRepository ProductRepository { get; }

        public ISupplyRepository SupplyRepository { get; }

        public IOrderRepository OrderRepository { get; }

        public Product RegisterProduct(string name)
        {
            Product newProduct = new Product.ProductBuilder()
                .WithName(name)
                .WithId(_issuedProductId++)
                .Build();

            ProductRepository.Save(newProduct);
            return newProduct;
        }

        public void AddProduct(Shop shop, Product product, int price, int quantity)
        {
            AddProducts(shop, new List<Product> { product }, new List<int> { price }, new List<int> { quantity });
        }

        public void AddProducts(Shop shop, List<Product> products, List<int> prices, List<int> quantities)
        {
            if (products.Count != prices.Count || products.Count != quantities.Count)
                throw new ProductException("Not enough data!");

            var supplyItems = new List<SupplyItem>();
            for (int i = 0; i < products.Count; i++)
            {
                ProductRepository.CheckProduct(products[i].Id);
                Product recentProduct = ProductRepository.FindProduct(products[i].Id, shop.Id);
                int newQuantity = quantities[i];
                if (recentProduct != null)
                {
                    newQuantity += recentProduct.Quantity;
                    ProductRepository.Delete(products[i].Id, shop.Id);
                }

                Product newProduct = products[i].ToBuilder()
                    .WithPrice(prices[i])
                    .WithShopId(shop.Id)
                    .WithQuantity(newQuantity)
                    .Build();

                ProductRepository.Save(newProduct);
                var supplyItem = new SupplyItem(products[i], prices[i], quantities[i]);
                supplyItems.Add(supplyItem);
            }

            var supply = new Supply(new List<SupplyItem>(supplyItems), _issuedSupplyId++);
            SupplyRepository.Save(supply);
        }

        public int PurchaseProduct(ref Customer customer, Shop shop, Product product, int amount)
        {
            return PurchaseProducts(ref customer, shop, new List<Product> { product }, new List<int> { amount });
        }

        public int PurchaseProducts(ref Customer customer, Shop shop, List<Product> products, List<int> amounts)
        {
            if (products.Count != amounts.Count)
                throw new ProductException("Not enough data!");

            var orderItems = new List<OrderItem>();
            int totalExpenses = 0;
            for (int i = 0; i < products.Count; i++)
            {
                ProductRepository.CheckProduct(products[i].Id);
                Product recentProduct = ProductRepository.GetProduct(products[i].Id, shop.Id);

                if (recentProduct.Quantity < amounts[i])
                    throw new ProductException("Not enough products!");

                int expenses = recentProduct.Price * amounts[i];
                if (customer.Cash < expenses)
                    throw new ProductException("Not enough cash!");

                ProductRepository.Delete(recentProduct.Id, recentProduct.ShopId);

                recentProduct = recentProduct.ToBuilder()
                    .WithQuantity(recentProduct.Quantity - amounts[i])
                    .Build();

                ProductRepository.Save(recentProduct);
                customer = new Customer(customer.Cash - expenses);
                totalExpenses += expenses;

                var orderItem = new OrderItem(products[i], amounts[i]);
                orderItems.Add(orderItem);
            }

            var order = new Order(new List<OrderItem>(orderItems), _issuedOrderId++);
            OrderRepository.Save(order);

            return totalExpenses;
        }
    }
}