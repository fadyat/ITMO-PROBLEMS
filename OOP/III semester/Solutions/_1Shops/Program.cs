using System;
using System.Collections.Generic;
using Shops.Classes;

namespace Shops
{
    internal static class Program
    {
        private static void Main()
        {
            var shopManager = new ShopManager();

            Shop shop1 = shopManager.CreateShop("Lenta");
            shop1.RegisterProduct(new List<string> { "milk", "sweet", });
            var products1 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(25, 19.99)),
            };
            shop1.AddProducts(products1);
            var products2 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(25, 40)),
                (new Product("sweet"), new ProductInfo(4, 4)),
            };
            shop1.AddProducts(products2);
            Shop shop2 = shopManager.CreateShop("Diksi");
            shop2.RegisterProduct(new List<string> { "milk", "pancakes", "sweet", });
            var products3 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(30, 30)),
                (new Product("pancakes"), new ProductInfo(5, 100.33)),
                (new Product("sweet"), new ProductInfo(200, 8)),
            };
            shop2.AddProducts(products3);
            var sergo = new Customer("Sergo", 10000);
            var products4 = new List<(Product, uint cnt)>
            {
                (new Product("milk"), 1),
                (new Product("sweet"), 2),
            };
            Console.WriteLine("--------------------------");
            shop1.PurchaseProduct(ref sergo, products4);
            Console.WriteLine(sergo);
            Console.WriteLine("--------------------------");
            shop1.PurchaseProduct(ref sergo, products4);
            Console.WriteLine(sergo);
            Console.WriteLine("--------------------------");
        }
    }
}
