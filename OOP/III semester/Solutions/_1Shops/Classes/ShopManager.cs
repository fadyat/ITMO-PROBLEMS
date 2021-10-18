using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Exceptions;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class ShopManager : IShopManager
    {
        private readonly List<Shop> _registeredShops;
        private List<Product> _registeredProducts;
        private uint _issuedShopId;
        private uint _issuedProductId;

        public ShopManager()
        {
            _registeredShops = new List<Shop>();
            _registeredProducts = new List<Product>();
            _issuedShopId = 100000;
            _issuedProductId = 100000;
        }

        public Shop CreateShop(string shopName)
        {
            Shop newShop = new Shop.ShopBuilder()
                .WithName(shopName)
                .WithId(_issuedShopId++)
                .Build();

            _registeredShops.Add(newShop); // ???
            return newShop;
        }

        public Product RegisterProduct(string productNameForRegistration)
        {
            List<Product> registered = RegisterProducts(new List<string> { productNameForRegistration });
            return registered[0];
        }

        public List<Product> RegisterProducts(IEnumerable<string> productNamesForRegistration)
        {
            var registeredProducts = productNamesForRegistration
                    .Select(productName => new Product.ProductBuilder()
                        .WithName(productName)
                        .WithId(_issuedProductId++)
                        .Build())
                    .ToList();

            _registeredProducts = _registeredProducts.Concat(registeredProducts).ToList(); // ???
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
                if (!_registeredProducts.Contains(productToAdd[i]))
                    throw new ShopException("This product hasn't been registered!");

                shop = shop.ToBuilder()
                    .AddProduct(productToAdd[i], productInfo[i])
                    .Build();
            }

            return shop.StoredProducts;
        }

        public uint PurchaseProducts(ref Customer customer, Shop shop, List<Product> productToPurchase, List<uint> quantities)
        {
            if (_registeredShops.Contains(shop))
                throw new ShopException("This shop hasn't been registered!");

            uint prevMoney = customer.Money;
            for (int i = 0; i < productToPurchase.Count; i++)
            {
                if (!_registeredProducts.Contains(productToPurchase[i]))
                    throw new ShopException("This product hasn't been registered!");

                customer = customer.ToBuilder()
                    .PurchaseProduct(ref shop, productToPurchase[i], quantities[i])
                    .Build();
            }

            return prevMoney - customer.Money;
        }

        public Shop FindCheapestShop(List<Product> productsToBuyCheap, List<uint> quantities)
        {
            if (productsToBuyCheap.Any(product => !_registeredProducts.Contains(product)))
                throw new ShopException("This product hasn't been registered!");

            uint lowestPrice = (uint)1e9;
            Shop shopWithLowestPrice = null;
            foreach (Shop shop in _registeredShops)
            {
                uint currentPrice = 0;
                bool isCorrectShop = true;
                for (int i = 0; i < productsToBuyCheap.Count; i++)
                {
                    int fakeSpendingMoney = shop.StoredProducts.FakeBuy(productsToBuyCheap[i], quantities[i]);
                    if (fakeSpendingMoney == -1)
                        isCorrectShop = false;

                    currentPrice += (uint)fakeSpendingMoney;
                }

                if (!isCorrectShop || currentPrice >= lowestPrice)
                    continue;

                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void Info()
        {
            Console.WriteLine(" - Registered products:");
            foreach (Product product in _registeredProducts)
                Console.WriteLine(product);

            Console.WriteLine();
            Console.WriteLine(" - Created shops:");
            foreach (Shop shop in _registeredShops)
                Console.WriteLine(shop);
        }
    }
}