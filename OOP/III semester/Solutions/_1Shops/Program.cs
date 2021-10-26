using System;
using System.Collections.Generic;
using Shops.Classes;
using Shops.Repositories;
using Shops.Services;

namespace Shops
{
    internal static class Program
    {
        private static void Main()
        {
            var productService = new ProductService(
                new ProductRepository(),
                new OrderRepository(),
                new SupplyRepository());

            var shopService = new ShopService(
                new ShopRepository(),
                productService.ProductRepository);

            Shop shop = shopService.CreateShop("shop1");
            Product product1 = productService.RegisterProduct("product1");
            Product product2 = productService.RegisterProduct("product2");
            Product product3 = productService.RegisterProduct("product3");
            productService.AddProduct(shop, product1, 10, 10);
            productService.AddProduct(shop, product1, 20, 20);
            productService.AddProduct(shop, product2, 30, 30);
            productService.AddProduct(shop, product3, 40, 40);
            productService.AddProducts(
                shop,
                new List<Product>
                {
                    product1,
                    product2,
                },
                new List<int>
                {
                    50,
                    50,
                },
                new List<int>
                {
                    30,
                    40,
                });
            productService.ProductRepository.Print();
            Console.WriteLine();

            productService.SupplyRepository.Print();
            Console.WriteLine();

            var me = new Customer(10000);
            Console.WriteLine(me.Cash);
            productService.PurchaseProduct(ref me, shop, product1, 5);
            productService.PurchaseProduct(ref me, shop, product2, 5);
            productService.PurchaseProducts(
                ref me,
                shop,
                new List<Product>
                {
                    product1,
                    product2,
                },
                new List<int>
                {
                    20,
                    20,
                });

            productService.OrderRepository.Print();
            Console.WriteLine(me.Cash);
        }
    }
}