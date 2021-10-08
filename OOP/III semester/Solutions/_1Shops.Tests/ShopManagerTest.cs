using System.Collections.Generic;
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
        
        private static readonly object[] RegisterData = 
        {
            new object[] {new List<string> {"apple","apple"}},
            
            new object[] {new List<string> {"pineapple", "pineapple"}}
        };
        
        [TestCaseSource(nameof(RegisterData))]
        public void RegisterEqualProducts_ThrowException(List<string> products)
        {
            Assert.Catch<ShopException>(() =>
            {
                Shop shop1 = _shopManager.CreateShop("Lenta");
                shop1.RegisterProduct(products);
            });
        }

        private static readonly object[] AddData = 
        {
            new object[] {"apple", new List<ProductInfo>
            {
                new ProductInfo(2, 1000),
                new ProductInfo(4, 1500),
                new ProductInfo(7, 2000)
            }},
            
            new object[] {"cake", new List<ProductInfo>
            {
                new ProductInfo(6, 3000),
                new ProductInfo(4, 5000),
                new ProductInfo(20, 2500)
            }}
        };

        [TestCaseSource(nameof(AddData))]
        public void AddingEqualsProductsToTheShop(string productName, List<ProductInfo> info)
        {
            Shop shop1 = _shopManager.CreateShop("Lenta");
            shop1.RegisterProduct(productName);
            uint totalCnt = 0;
            uint lastPrice = 0;
            foreach (ProductInfo newInfo in info)
            {
                shop1.AddProducts(new Product(productName), newInfo);
                totalCnt += newInfo.Cnt;
                lastPrice = newInfo.Price;
            }
            var product = new Product(productName);
            Assert.AreEqual(totalCnt, shop1.StoredProducts[product].Cnt);
            Assert.AreEqual(lastPrice, shop1.StoredProducts[product].Price);
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
        public void CheapestShopFinding(List<(string, Dictionary<Product, ProductInfo>)> shopsInfo, 
            List<(Product, uint cheapCnt)> productsToBuyCheap) 
        {
            /* in tests shop0 is always cheapest */
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
                new Customer("customer0", 100000),
                new Dictionary<Product, ProductInfo> {
                    {new Product("milk"), new ProductInfo(12, 4000)},
                    {new Product("cake"), new ProductInfo(30, 3000)}
                },
                new List<(Product, uint purchaseCnt)> {
                    (new Product("milk"), 10),
                    (new Product("cake"), 15)
                }
            },
            
            new object[] {
                new Customer("customer1", 500000),
                new Dictionary<Product, ProductInfo> {
                    {new Product("milk"), new ProductInfo(40, 4100)}
                },
                new List<(Product, uint purchaseCnt)> {
                    (new Product("milk"), 20)
                }
            }
        };
        
        [TestCaseSource(nameof(PurchaseData))]
        public void PurchaseProduct(Customer customer, Dictionary<Product, ProductInfo> products,
            List<(Product, uint purchaseCnt)> productsToPurchase)
        {
            Shop shop = _shopManager.CreateShop("shop0");
            var prevCnt = new Dictionary<Product, uint>();
            foreach ((Product product,  ProductInfo productInfo) in products)
            {
                shop.RegisterProduct(product.Name);
                shop.AddProducts(product, productInfo);
                prevCnt.Add(product, productInfo.Cnt);
            }
            uint lostMoney = 0;
            foreach ((Product product, uint purchaseCnt) in productsToPurchase)
            {
                lostMoney += products[product].Price * purchaseCnt;
            }

            uint prevMoney = customer.Money;
            shop.PurchaseProduct(ref customer, productsToPurchase);
            Assert.AreEqual(lostMoney, prevMoney - customer.Money);
            foreach ((Product product, uint purchaseCnt) in productsToPurchase)
            {
                Assert.AreEqual(purchaseCnt, prevCnt[product] - shop.StoredProducts[product].Cnt);
            }
        }
    }
}