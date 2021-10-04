using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shops.Classes;
using Shops.Exceptions;

namespace Shops.Tests
{
    public class ShopManagerTest
    {
        private ShopManager _shopManager;

        [SetUp] public void SetUp() { _shopManager = new ShopManager(); }
        
        [Test] public void RegisterEqualProducts_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                Shop shop1 = _shopManager.CreateShop("Lenta", "Pushkin1");
                _shopManager.RegisterProduct(shop1, new List<string> { "pineapple", "pineapple" });
            });
        }
        
        [Test] public void AddingEqualsProductsToTheShop()
        {
            Shop shop1 = _shopManager.CreateShop("Lenta", "Pushkin1");
            _shopManager.RegisterProduct(shop1, new List<string> { "apple" });
            var apple = new Product("apple");
            _shopManager.AddProducts(shop1, new List<(Product, ProductInfo)>
            {
                (apple, new ProductInfo(10, 5.99)),
                (new Product("apple"), new ProductInfo(10, 4.99))
            });
            Assert.True(shop1.StoredProducts.Count == 1 && shop1.StoredProducts[apple].Cnt == 20);
        }

        [Test] public void CheapestShopFinding()
        {
            Shop shop1 = _shopManager.CreateShop("Lenta", "Pushkin1");
            _shopManager.RegisterProduct(shop1, new List<string> { "apple", "sweet" });
            _shopManager.AddProducts(shop1, new List<(Product, ProductInfo)>
            {
                (new Product("apple"), new ProductInfo(10, 10)),
                (new Product("sweet"), new ProductInfo(10, 20))
            });
            
            Shop shop2 = _shopManager.CreateShop("Lenta", "Pushkin1");
            _shopManager.RegisterProduct(shop2, new List<string> { "apple", "sweet" });
            _shopManager.AddProducts(shop2, new List<(Product, ProductInfo)>
            {
                (new Product("apple"), new ProductInfo(10, 20)),
                (new Product("sweet"), new ProductInfo(10, 10))
            });

            Shop shop3 = _shopManager.CheapProductSearch(new List<(Product, uint cheapCnt)>
            {
                (new Product("apple"), 4),
                (new Product("sweet"), 6)
            });
            Assert.True(shop2.Id == shop3.Id);
        }

        [Test] public void PurchaseProduct()
        {
            Shop shop1 = _shopManager.CreateShop("Lenta", "Pushkin1");
            _shopManager.RegisterProduct(shop1, new List<string> { "apple", "sweet" });
            _shopManager.AddProducts(shop1, new List<(Product, ProductInfo)>
            {
                (new Product("apple"), new ProductInfo(10, 10)),
                (new Product("sweet"), new ProductInfo(10, 20))
            });
            
            Shop shop2 = _shopManager.CreateShop("Lenta", "Pushkin1");
            _shopManager.RegisterProduct(shop2, new List<string> { "apple", "sweet" });
            _shopManager.AddProducts(shop2, new List<(Product, ProductInfo)>
            {
                (new Product("apple"), new ProductInfo(10, 20)),
                (new Product("sweet"), new ProductInfo(10, 10))
            });
            var customer1 = new Customer("customer1", 1000);
            var customer2 = new Customer("customer2", 1000);
            _shopManager.PurchaseProduct(customer1, shop1, new List<(Product, uint purchaseCnt)>
            {
                (new Product("apple"), 5),
                (new Product("sweet"), 5)
            });
            _shopManager.PurchaseProduct(customer2, shop2, new List<(Product, uint purchaseCnt)>
            {
                (new Product("apple"), 5),
                (new Product("sweet"), 5)
            });
            Assert.True(Math.Abs(customer1.Money - 850) == 0 && Math.Abs(customer2.Money - 850) == 0);
        }
    }
}