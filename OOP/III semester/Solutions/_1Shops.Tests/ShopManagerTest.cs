using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Shops.Classes;
using Shops.Services;

namespace Shops.Tests
{
    [TestFixture]
    public class ShopManagerTest
    {
        private ShopService _shopService;
        private ProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _shopService = new ShopService();
            _productService = new ProductService();
        }

        private static readonly object[] ShopCreationData =
        {
            new object[] {new List<string> {"shop1"}},
            new object[] {new List<string> {"shop1", "shop1"}},
            new object[] {new List<string> {"shop1", "shop2", "shop3"}},
        };

        [TestCaseSource(nameof(ShopCreationData))]
        public void CreateShop(List<string> shopNames)
        {
            IEnumerable<Shop> shops = shopNames.Select(shopName => _shopService.CreateShop(shopName))
                .ToList();

            Assert.True(shops.SequenceEqual(_shopService.Shops));
        }

        private static readonly object[] ProductRegistrationData =
        {
            new object[] {"shop1", new List<string> {"apple", "apple"}},
            new object[] {"shop2", new List<string> {"apple", "banana"}},
        };

        [TestCaseSource(nameof(ProductRegistrationData))]
        public void RegisterProduct(string shopName, List<string> productNames)
        {
            var products = productNames.Select(productName => _productService.RegisterProduct(productName))
                .ToList();

            Assert.True(products.SequenceEqual(_productService.Registrations.GetAll()));
        }

        private static readonly object[] AddingData =
        {
            new object[]
            {
                new List<(string, List<(int price, int quantity)> supplies)>
                {
                    ("product1", new List<(int price, int quantity)>
                    {
                        (10, 100),
                    }),
                    ("product2", new List<(int price, int quantity)>
                    {
                        (41, 200),
                    }),
                    ("product3", new List<(int price, int quantity)>
                    {
                        (60, 300),
                    })
                }
            },
            new object[]
            {
                new List<(string, List<(int price, int quantity)> supplies)>
                {
                    ("product1", new List<(int price, int quantity)>
                    {
                        (10, 100),
                        (20, 200),
                        (10, 100),
                        (20, 200),
                    }),
                    ("product2", new List<(int price, int quantity)>
                    {
                        (41, 200),
                        (42, 4000),
                    }),
                }
            },
        };

        [TestCaseSource(nameof(AddingData))]
        public void AddProducts(List<(string productName, List<(int price, int quantity)> supplies)> products)
        {
            Shop shop = _shopService.CreateShop("shop1");
            var lastStage = new List<Product>();
            foreach ((string name, List<(int price, int quantity)> data) in products)
            {
                Product currentProduct = _productService.RegisterProduct(name);
                int lastPrice = 0;
                int totalQuantity = 0;
                foreach ((int price, int quantity) in data)
                {
                    _productService.AddProduct(shop, currentProduct, price, quantity);
                    lastPrice = price;
                    totalQuantity += quantity;
                }

                Product productCondition =
                    _productService.ProductRepository.FindProduct(currentProduct.Id, shop.Id);
                lastStage.Add(productCondition);

                Assert.AreEqual(productCondition.Price, lastPrice);
                Assert.AreEqual(productCondition.Quantity, totalQuantity);
            }

            Assert.True(lastStage.SequenceEqual(_productService.ProductRepository.GetAll()));
        }

        private static readonly object[] PurchaseData =
        {
            new object[]
            {
                new Customer(1000),
                "apple",
                100,
                10,
                5
            },
            new object[]
            {
                new Customer(1000),
                "book",
                1000,
                2,
                1
            },
        };

        [TestCaseSource(nameof(PurchaseData))]
        public void PurchaseProduct(Customer customer, string productName, int price,
            int quantity, int amount)
        {
            Shop shop = _shopService.CreateShop("shop1");
            Product product = _productService.RegisterProduct(productName);
            _productService.AddProduct(shop, product, price, quantity);
            int prevCash = customer.Cash;
            int expenses = _productService.PurchaseProduct(ref customer, shop, product, amount);
            Product productCondition = _productService.ProductRepository.GetProduct(product.Id, shop.Id);
            Assert.AreEqual(quantity - amount, productCondition.Quantity);
            Assert.AreEqual(expenses, price * amount);
            Assert.AreEqual(customer.Cash + expenses, prevCash);
        }

        private static readonly object[] PurchaseDataThrowException =
        {
            new object[]
            {
                new Customer(1000),
                "apple",
                120,
                10,
                9
            },
            new object[]
            {
                new Customer(1000),
                "book",
                100,
                2,
                3
            },
            new object[]
            {
                new Customer(1000),
                "orange",
                1000,
                2,
                3
            },
        };

        [TestCaseSource(nameof(PurchaseDataThrowException))]
        public void PurchaseProduct_ThrowException(Customer customer, string productName, int price,
            int quantity, int amount)
        {
            Assert.Catch<Exception>(() =>
            {
                Shop shop = _shopService.CreateShop("shop1");
                Product product = _productService.RegisterProduct(productName);
                _productService.AddProduct(shop, product, price, quantity);
                _productService.PurchaseProduct(ref customer, shop, product, amount);
            });
        }

        private static readonly object[] FindCheapestShopIdData =
        {
            new object[]
            {
                new List<string> {"apple", "bread"},
                new List<(string, List<(int price, int quantity)> supplies)>
                {
                    ("shop1", new List<(int price, int quantity)>
                    {
                        (10, 100),
                        (20, 300),
                    }),
                    ("shop2", new List<(int price, int quantity)>
                    {
                        (11, 100),
                        (21, 300),
                    })
                },
                new List<int> {1},
                new List<int> {10},
            }
        };

        [TestCaseSource(nameof(FindCheapestShopIdData))]
        public void FindTheCheapestShopId(
            List<string> register, List<(string, List<(int, int)>)> shops, 
            List<int> whichProductsBuy, List<int> amounts)
        {
            // `shop0`.id - the cheapest
            foreach (string reg in register)
            {
                _productService.RegisterProduct(reg);
            }

            var registered = _productService.Registrations.GetAll().ToList();
            foreach ((string name, List<(int price, int quantity)> lst) in shops)
            {
                Shop shop = _shopService.CreateShop(name);
                for (int i = 0; i < lst.Count; i++)
                {
                    _productService.AddProduct(shop, registered[i], lst[i].price, lst[i].quantity);
                }
            }

            var products = _productService.ProductRepository.GetAll().ToList();
            var buyThis = whichProductsBuy.Select(addThis => products[addThis]).ToList();
            var id = _shopService.Shops.ToList();
            Assert.AreEqual(_productService.CheapestShopIdFinding(buyThis, amounts), id[0].Id);
        }
    }
}