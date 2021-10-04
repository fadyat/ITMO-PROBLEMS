/*using System.Collections.Generic;
using NUnit.Framework;
using _1Shops.Classes;
using _1Shops.Exceptions;

namespace _1Shops.Tests
{
    public class ShopManagerTest
    {
        private ShopManager _shopManager;

        [SetUp]
        public void SetUp() { _shopManager = new ShopManager(); }

        
        
        [Test]
        public void RegisterEqualProducts_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                ShopManager.RegisterProduct("pineapple");
                ShopManager.RegisterProduct("pineapple");
            });
        }

        private static readonly object[] TestCases =
        {
            new List<string> {"milk", "water", "apple"}
        };
        
        [Test, TestCaseSource(nameof(TestCases))]
        public void AddingProductsToTheShop_ThrowException(List<string> products)
        {
            Assert.Catch<ShopException>(() =>
            {
                _shopManager.CreateShop("Lenta", "Lermontova 1");
                foreach (string pr in products)
                    ShopManager.RegisterProduct(pr);
                
            });
        }
    }
}*/