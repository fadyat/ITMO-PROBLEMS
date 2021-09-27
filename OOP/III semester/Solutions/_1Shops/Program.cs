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
            var products1 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(25)),
                (new Product("beer"), new ProductInfo(50)),
            };
            var prices1 = new List<double> { 19.99, 59.99 };
            shopManager.AddProducts(shop1, products1, prices1);

            Shop shop2 = shopManager.CreateShop("Diksi", "Lermontova_5");
            var products2 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(30)),
                (new Product("pancakes"), new ProductInfo(5)),
                (new Product("sweet"), new ProductInfo(200)),
            };
            var prices2 = new List<double> { 29.99, 100.33, 7.00 };
            shopManager.AddProducts(shop2, products2, prices2);
            var sergo = new Customer("Sergo", 1000);
            var products3 = new List<(Product, ProductInfo)>
            {
                (new Product("milk"), new ProductInfo(1)),
            };
            shopManager.PurchaseProduct(sergo, shopManager.CheapProductSearch(products3), products3);
            /* Console.WriteLine(sergo.Money); */
            shopManager.Info();
        }
    }
}
