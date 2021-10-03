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
            Shop shop1 = shopManager.CreateShop("Lenta", "Pushkin_Square_12");
            shopManager.RegisterProduct(shop1, new List<string> { "milk", "beer", "apple", "sweet", });
            var products1 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(25, 19.99)),
                (new Product("beer"), new ProductInfo(50, 59.99)),
            };
            shopManager.AddProducts(shop1, products1);
            var products2 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(25, 39.99)),
                (new Product("sweet"), new ProductInfo(2, 1.99)),
            };
            shopManager.AddProducts(shop1, products2);
            Shop shop2 = shopManager.CreateShop("Diksi", "Lermontova_5");
            shopManager.RegisterProduct(shop2, new List<string> { "???", "rat", "milk", "pancakes", "sweet", });
            var products3 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(30, 29.99)),
                (new Product("pancakes"), new ProductInfo(5, 100.33)),
                (new Product("sweet"), new ProductInfo(200, 8.00)),
            };
            shopManager.AddProducts(shop2, products3);
            var sergo = new Customer("Sergo", 1000);
            var products4 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(1)),
                (new Product("sweet"), new ProductInfo(2)),
            };
            shopManager.PurchaseProduct(sergo, shopManager.CheapProductSearch(products4), products4);
            Console.WriteLine(sergo.Money);
            shopManager.Info();
        }
    }
}
