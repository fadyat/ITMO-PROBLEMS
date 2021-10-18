using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shops.Classes;
using Shops.Exceptions;

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
            List<Product> registeredProducts = _shopManager.RegisterProduct(productName);
            Product myProduct = registeredProducts[0];
            uint totalCnt = 0;
            uint lastPrice = 0;
            foreach (ProductInfo newInfo in info)
            {
                _shopManager.AddProduct(shop1, myProduct, newInfo);
                totalCnt += newInfo.Quantity;
                lastPrice = newInfo.Price;
            }
            Assert.AreEqual(totalCnt, shop1.StoredProducts.GetProductInfo(myProduct).Quantity);
            Assert.AreEqual(lastPrice, shop1.StoredProducts.GetProductInfo(myProduct).Price);
        }

        private static readonly object[] CheapData =
        {
            new object[] { 
                new List<(string, Dictionary<Product, ProductInfo>)> {
                    ("shop0", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("milk"), new ProductInfo(10, 5000)},
                        {new Product("apple"), new ProductInfo(10, 800)}
                    }),
                    ("shop1", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("milk"), new ProductInfo(10, 5100)},
                        {new Product("apple"), new ProductInfo(10, 810)}
                    })
                },
                new List<(Product, uint cheapCnt)> {
                    (new Product("milk"), 5),
                    (new Product("apple"), 5)
                }
            },
            
            new object[] { 
                new List<(string, Dictionary<Product, ProductInfo>)> {
                    ("shop0", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("milk"), new ProductInfo(10, 5000)}
                    }),
                    ("shop1", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("apple"), new ProductInfo(10, 5100)}
                    })
                },
                new List<(Product, uint cheapCnt)> {
                    (new Product("milk"), 5)
                }
            }, 
            
            new object[] { 
                new List<(string, Dictionary<Product, ProductInfo>)> {
                    ("shop0", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("milk"), new ProductInfo(10, 5900)},
                        {new Product("sweet"), new ProductInfo(10, 200)}
                    }),
                    ("shop1", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("milk"), new ProductInfo(10, 5200)},
                        {new Product("sweet"), new ProductInfo(3, 1000)}
                    }),
                    ("shop2", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("apple"), new ProductInfo(10, 5100)},
                        {new Product("sweet"), new ProductInfo(3, 100)}
                    }),
                    ("shop3", new Dictionary<Product, ProductInfo>
                    {
                        {new Product("milk"), new ProductInfo(10, 5100)},
                        {new Product("sweet"), new ProductInfo(2, 100)}
                    })
                },
                new List<(Product, uint cheapCnt)> {
                    (new Product("milk"), 3),
                    (new Product("sweet"), 3)
                }
            }
        };
        
        [TestCaseSource(nameof(CheapData))]
        public void CheapestShopFinding(List<(string, List<(string, List<ProductInfo>)>)> shopsWithProductsToRegister, 
            List<(Product, uint quantities)> productsToBuyCheap)
        {
            foreach ((string shopName, Dictionary<Product, ProductInfo> products) in shopsInfo)
            {
                Shop shop = _shopManager.CreateShop(shopName);
                foreach ((Product product, ProductInfo productInfo) in products)
                {
                    shop.RegisterProduct(product.Name);
                    shop.AddProducts(product, productInfo);
                }
            }
            Shop cheapShop = _shopManager.CheapProductSearch(productsToBuyCheap);
            Assert.AreEqual(cheapShop.Name, "shop0");
        }

        private static readonly object[] PurchaseData =
        {
            new object[] {
                "shop1",
                new Customer.CustomerBuilder()
                    .WithName("customer1")
                    .WithMoney(10000)
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
            List<Product> registerProducts = _shopManager.RegisterProducts(productsToRegister);
            var pickedProducts = registerProducts
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