using System;
using System.Collections.Generic;
using System.Linq;

namespace Shops.Classes
{
    public class ShopManager
    {
        private List<Shop> _totalCreatedShops;
        private List<Product> _totalRegisteredProducts;
        private uint _issuedShopId;
        private uint _issuedProductId;

        public ShopManager()
        {
            _totalCreatedShops = new List<Shop>();
            _totalRegisteredProducts = new List<Product>();
            _issuedShopId = 100000;
            _issuedProductId = 100000;
        }

        public Shop CreateShop(string shopName)
        {
            Shop newShop = new Shop.ShopBuilder()
                .WithName(shopName)
                .WithId(_issuedShopId++)
                .Build();

            _totalCreatedShops.Add(newShop); // ???
            return newShop;
        }

        public List<Product> RegisterProduct(string productNameForRegistration)
        {
            return RegisterProducts(new List<string> { productNameForRegistration });
        }

        public List<Product> RegisterProducts(IEnumerable<string> productNamesForRegistration)
        {
            var registeredProducts = productNamesForRegistration
                    .Select(productName => new Product.ProductBuilder()
                        .WithName(productName)
                        .WithId(_issuedProductId++)
                        .Build())
                    .ToList();

            _totalRegisteredProducts = _totalRegisteredProducts.Concat(registeredProducts).ToList(); // ???
            return registeredProducts;
        }

        public StoredProducts AddProduct(Shop shop, Product productToAdd, ProductInfo productInfo)
        {
            return AddProducts(shop, new List<Product> { productToAdd }, new List<ProductInfo> { productInfo });
        }

        public StoredProducts AddProducts(Shop shop, List<Product> productToAdd, List<ProductInfo> productInfo)
        {
            for (int i = 0; i < productToAdd.Count; i++)
            {
                shop = shop.ToBuilder()
                    .AddProduct(productToAdd[i], productInfo[i])
                    .Build();
            }

            return shop.StoredProducts;
        }

        public uint PurchaseProducts(ref Customer customer, Shop shop, List<Product> productToPurchase, List<uint> quantities)
        {
            uint prevMoney = customer.Money;
            for (int i = 0; i < productToPurchase.Count; i++)
            {
                customer = customer.ToBuilder()
                    .PurchaseProduct(ref shop, productToPurchase[i], quantities[i])
                    .Build();
            }

            return prevMoney - customer.Money;
        }

        public Shop FindCheapestShop(List<Product> productsToBuyCheap, List<uint> quantities)
        {
            uint lowestPrice = (uint)1e9;
            Shop shopWithLowestPrice = null;
            foreach (Shop shop in _totalCreatedShops)
            {
                uint currentPrice = 0;
                bool correctShop = true;
                for (int i = 0; i < productsToBuyCheap.Count; i++)
                {
                    int fakeSpendingMoney = shop.StoredProducts.FakeBuy(productsToBuyCheap[i], quantities[i]);
                    if (fakeSpendingMoney == -1)
                        correctShop = false;

                    currentPrice += (uint)fakeSpendingMoney;
                }

                if (!correctShop || currentPrice >= lowestPrice)
                    continue;

                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void Info()
        {
            Console.WriteLine(" - Registered products:");
            foreach (Product product in _totalRegisteredProducts)
                Console.WriteLine(product);

            Console.WriteLine();
            Console.WriteLine(" - Created shops:");
            foreach (Shop shop in _totalCreatedShops)
                Console.WriteLine(shop);
        }
    }
}