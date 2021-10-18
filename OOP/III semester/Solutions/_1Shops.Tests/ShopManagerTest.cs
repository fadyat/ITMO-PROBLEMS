using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shops.Classes;

namespace Shops.Tests
{
    [TestFixture]
    public class ShopManagerTest
    {
        private ShopManager _shopManager;

        [SetUp]
        public void SetUp()
        {
            _shopManager = new ShopManager();
        }

        private static readonly object[] AddData = 
        {
            new object[] {"apple", new List<ProductInfo>
            {
                new ProductInfo.ProductInfoBuilder()
                    .WithQuantity(2)
                    .WithPrice(1000)
                    .Build(),
                new ProductInfo.ProductInfoBuilder()
                    .WithQuantity(40)
                    .WithPrice(2000)
                    .Build(),
            }}
        };

        [TestCaseSource(nameof(AddData))]
        public void AddingEqualsProductsToTheShop(string productName, List<ProductInfo> info)
        {
            Shop shop1 = _shopManager.CreateShop("Lenta");
            Product myProduct = _shopManager.RegisterProduct(productName);
            uint totalCnt = 0;
            uint lastPrice = 0;
            foreach (ProductInfo newInfo in info)
            {
                _shopManager.AddProduct(shop1, myProduct, newInfo);
                totalCnt += newInfo.Quantity;
                lastPrice = newInfo.Price;
            }
            Assert.AreEqual(totalCnt, shop1.GetProductInfo(myProduct).Quantity);
            Assert.AreEqual(lastPrice, shop1.GetProductInfo(myProduct).Price);
        }

        private static readonly object[] CheapData =
        {
            new object[] {
                new List<(string, List<ProductInfo>)> {
                    ("shop0", new List<ProductInfo>
                    {
                        new ProductInfo.ProductInfoBuilder()
                            .WithPrice(4000)
                            .WithQuantity(25)
                            .Build(),
                        new ProductInfo.ProductInfoBuilder()
                            .WithPrice(600)
                            .WithQuantity(55)
                            .Build()
                    }),
                    ("shop1", new List<ProductInfo>
                    {
                        new ProductInfo.ProductInfoBuilder()
                            .WithPrice(5000)
                            .WithQuantity(25)
                            .Build(),
                        new ProductInfo.ProductInfoBuilder()
                            .WithPrice(700)
                            .WithQuantity(55)
                            .Build()
                    }),
                },
                new List<string> { "milk", "apple", "juice" },
                new List<int> { 1, 2 },
                new List<uint> { 10, 20 }
            }
        };
        
        [TestCaseSource(nameof(CheapData))]
        public void CheapestShopFinding(List<(string shopName, List<ProductInfo> hisProductInfo)> shopsWithProductsInfo, 
            List<string> productsToRegister, List<int> whichProductsPick, List<uint> hisQuantities)
        {
            /* `shop0` always cheapest */
            List<Product> registeredProducts = _shopManager.RegisterProducts(productsToRegister);
            var pickedProducts = registeredProducts
                .Where((t, i) => whichProductsPick.Contains(i + 1))
                .ToList();
            
            foreach ((string shopName, List<ProductInfo> productInfo) in shopsWithProductsInfo)
            {
                Shop currentShop = _shopManager.CreateShop(shopName);
                _shopManager.AddProducts(currentShop, pickedProducts, productInfo);
            }

            Shop cheapestShop = _shopManager.FindingTheCheapestShop(pickedProducts, hisQuantities);
            Assert.AreEqual(cheapestShop.Name, "shop0");
        }

        private static readonly object[] PurchaseData =
        {
            new object[] {
                "shop1",
                new Customer.CustomerBuilder()
                    .WithName("customer1")
                    .WithMoney(20000)
                    .Build(),
                new List<string> { "apple", "banana", "pineapple" },
                new List<int> { 1, 3 },
                new List<ProductInfo>
                {
                    new ProductInfo.ProductInfoBuilder()
                        .WithQuantity(10)
                        .WithPrice(1000)
                        .Build(),
                    new ProductInfo.ProductInfoBuilder()
                        .WithQuantity(20)
                        .WithPrice(2000)
                        .Build()
                },
                new List<uint> { 7, 15 }
            }
        };
        
        [TestCaseSource(nameof(PurchaseData))]
        public void PurchaseProduct(string shopName, Customer customer,
            List<string> productsToRegister, List<int> whichProductsPick,
            List<ProductInfo> hisProductsInfo, List<uint> hisQuantities)
        {
            Shop shop = _shopManager.CreateShop(shopName);
            List<Product> registeredProducts = _shopManager.RegisterProducts(productsToRegister);
            var pickedProducts = registeredProducts
                .Where((t, i) => whichProductsPick.Contains(i + 1))
                .ToList();
            
            StoredProducts previousStored = _shopManager.AddProducts(shop, pickedProducts, hisProductsInfo);
            uint prevMoney = customer.Money;
            uint spentMoney = _shopManager.PurchaseProducts(ref customer, shop, pickedProducts, hisQuantities);
            
            Assert.AreEqual(spentMoney, prevMoney - customer.Money);

            for (int i = 0; i < hisProductsInfo.Count; i++)
            {
                hisProductsInfo[i] = hisProductsInfo[i].ToBuilder()
                    .WithQuantity(hisQuantities[i])
                    .Build();
            }
            _shopManager.AddProducts(shop, pickedProducts, hisProductsInfo);
            Assert.AreEqual(previousStored, shop.StoredProducts);
        }
    }
}