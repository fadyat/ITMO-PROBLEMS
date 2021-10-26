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
            var a = new ShopService(
                new ShopRepository(),
                new ProductService(
                    new ProductRepository(),
                    new OrderRepository(),
                    new SupplyRepository()));

            Shop shop = a.CreateShop("shop1");
            Product product1 = a.ProductService.RegisterProduct("product1");
            Product product2 = a.ProductService.RegisterProduct("product2");
            Product product3 = a.ProductService.RegisterProduct("product3");
            a.ProductService.AddProduct(shop, product1, 10, 10);
            a.ProductService.AddProduct(shop, product1, 20, 20);
            a.ProductService.AddProduct(shop, product2, 30, 30);
            a.ProductService.AddProduct(shop, product3, 40, 40);
            a.ProductService.AddProducts(
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
            a.ProductService.ProductRepository.Print();
            Console.WriteLine();

            a.ProductService.SupplyRepository.Print();
            Console.WriteLine();

            var me = new Customer(10000);
            Console.WriteLine(me.Cash);
            a.ProductService.PurchaseProduct(ref me, shop, product1, 5);
            a.ProductService.PurchaseProduct(ref me, shop, product2, 5);
            a.ProductService.PurchaseProducts(
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

            a.ProductService.OrderRepository.Print();
            Console.WriteLine(me.Cash);
        }
    }
}