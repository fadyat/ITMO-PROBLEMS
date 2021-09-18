using System.Collections.Generic;
using Shops.Classes;

namespace Shops
{
    internal class Program
    {
        private static void Main()
        {
            /*

             ShopManager ---- List<Shop> _shops + List<Product> _registeredProducts
             |
             |_ (CreateShop, RegisterProduct, CheapProductSearch, DeliverProducts, SetProductsPrices, PurchaseProduct)
                 |            |
                 |            |
                 |            |_ Product ---- _price + _name
                 |
                 |_ Shop ---- _id + _name + _address + Dictionary<Product, ProductQuantity> _storedProducts


             Customer ---- _name + _money + Dictionary<Product, ProductQuantity> _purchasedProducts

             */

            var shopManager = new ShopManager();
            shopManager.RegisterProduct("milk");
            shopManager.RegisterProduct("beer");
            shopManager.RegisterProduct("apple");
            shopManager.RegisterProduct("pancakes");
            shopManager.RegisterProduct("sweet");

            Shop shop1 = shopManager.CreateShop("Lenta", "Pushkin_Square_12");
            var products1 = new List<KeyValuePair<Product, ProductQuantity>>()
            {
                new KeyValuePair<Product, ProductQuantity>(new Product("milk"), new ProductQuantity(25)),
                new KeyValuePair<Product, ProductQuantity>(new Product("beer"), new ProductQuantity(50)),
            };
            var prices1 = new List<double>() { 19.99, 59.99 };
            shopManager.AddProducts(shop1, products1, prices1);

            Shop shop2 = shopManager.CreateShop("Diksi", "Lermontova_5");
            var products2 = new List<KeyValuePair<Product, ProductQuantity>>()
            {
                new KeyValuePair<Product, ProductQuantity>(new Product("milk"), new ProductQuantity(30)),
                new KeyValuePair<Product, ProductQuantity>(new Product("pancakes"), new ProductQuantity(5)),
                new KeyValuePair<Product, ProductQuantity>(new Product("sweet"), new ProductQuantity(200)),
            };
            var prices2 = new List<double>() { 29.99, 100.33, 7.00 };
            shopManager.AddProducts(shop2, products2, prices2);
            shopManager.LookThrow();
        }
    }
}
