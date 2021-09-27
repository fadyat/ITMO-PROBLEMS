using System;
using System.Collections.Generic;
using Shops.Classes;

namespace Shops
{
    internal class Program
    {
        private static void Main()
        {
            var shopManager = new ShopManager();
            shopManager.RegisterProduct("milk");
            shopManager.RegisterProduct("beer");
            shopManager.RegisterProduct("apple");
            shopManager.RegisterProduct("pancakes");
            shopManager.RegisterProduct("sweet");

            Shop shop1 = shopManager.CreateShop("Lenta", "Pushkin_Square_12");
            var products1 = new List<(Product, uint cnt, double price)>
            {
                (new Product("milk"), 25, 19.99),
                (new Product("beer"), 50, 59.99),
            };
            shopManager.AddProducts(shop1, products1);

            Shop shop2 = shopManager.CreateShop("Diksi", "Lermontova_5");
            var products2 = new List<(Product, uint cnt, double price)>
            {
                (new Product("milk"), 25, 29.99),
                (new Product("beer"), 2, 103.99),
                (new Product("sweet"), 50, 7.00),
            };
            shopManager.AddProducts(shop2, products2);
            var sergo = new Customer("Sergo", 1000);
            var products3 = new List<(Product, uint cnt)>
            {
                (new Product("milk"), 12),
                (new Product("sweet"), 50),
            };
            Shop kek = shopManager.CheapProductSearch(products3);
            Console.WriteLine(kek);
            Console.WriteLine();

            shopManager.PurchaseProduct(sergo, shopManager.CheapProductSearch(products3), products3);
            Console.WriteLine(sergo.Money);
            shopManager.Info();
        }
    }
}
