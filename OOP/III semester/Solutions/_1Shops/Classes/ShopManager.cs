using System;
using System.Collections.Generic;
using Shops.Exceptions;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class ShopManager : IShopManager
    {
        private readonly HashSet<Shop> _createdShops;
        private readonly HashSet<Product> _registeredProducts;
        private uint _spareId;

        public ShopManager()
        {
            _createdShops = new HashSet<Shop>();
            _registeredProducts = new HashSet<Product>();
            _spareId = 100000;
        }

        public Shop CreateShop(string shopName, string shopAddress)
        {
            var newShop = new Shop(_spareId++, shopName, shopAddress);
            _createdShops.Add(newShop);
            return newShop;
        }

        public Product RegisterProduct(string productName)
        {
            var newProduct = new Product(productName);
            if (_registeredProducts.Contains(newProduct))
                throw new ShopException($"Product {newProduct.Name} is already registered!");

            _registeredProducts.Add(newProduct);
            return newProduct;
        }

        public Shop CheapProductSearch(List<(Product, ProductInfo)> productsToBuyCheap)
        {
            double lowestPrice = 1e9;
            var shopWithLowestPrice = new Shop();
            foreach (Shop shop in _createdShops)
            {
                double currentPrice = 0;
                bool correctShop = true;
                foreach ((Product product, ProductInfo productInfo) in productsToBuyCheap)
                {
                    if (!_registeredProducts.Contains(product))
                        return null;

                    if (!shop.StoredProducts.ContainsKey(product) || shop.StoredProducts[product].Cnt < productInfo.Cnt)
                    {
                        correctShop = false;
                        break;
                    }

                    currentPrice += productInfo.Cnt * shop.StoredProducts[product].Price;
                }

                if (!correctShop || currentPrice >= lowestPrice) continue;

                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void AddProducts(Shop shop, List<(Product, ProductInfo)> products)
        {
            if (!_createdShops.Contains(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            foreach ((Product product, ProductInfo productInfo) in products)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (!shop.StoredProducts.ContainsKey(product))
                    shop.StoredProducts.Add(product, new ProductInfo(0, 0));

                shop.StoredProducts[product].Cnt += productInfo.Cnt;
                shop.StoredProducts[product].Price = productInfo.Price;
            }
        }

        public void PurchaseProduct(Customer customer, Shop shop, List<(Product, ProductInfo)> productsToPurchase)
        {
            if (!_createdShops.Contains(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            foreach ((Product product, ProductInfo productInfo) in productsToPurchase)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (shop.StoredProducts[product].Cnt >= productInfo.Cnt)
                {
                    if (customer.Money >= shop.StoredProducts[product].Price * productInfo.Cnt)
                    {
                        shop.StoredProducts[product].Cnt -= productInfo.Cnt;
                        customer.Money -= shop.StoredProducts[product].Price * productInfo.Cnt;
                    }
                    else
                    {
                        throw new ShopException(
                            $"Not enough money need more: {(shop.StoredProducts[product].Price * productInfo.Cnt) - customer.Money}");
                    }
                }
                else
                {
                    throw new ShopException(
                        $"Not enough products need more: {productInfo.Cnt - shop.StoredProducts[product].Cnt}");
                }
            }
        }

        public void Info()
        {
            Console.WriteLine("\n\nCreated shops:");
            foreach (Shop shop in _createdShops)
            {
                Console.WriteLine(" * " + shop);
                foreach ((Product product, ProductInfo productInfo) in shop.StoredProducts)
                {
                    Console.WriteLine("\t - " + product + " " + productInfo);
                }
            }

            Console.WriteLine("\n\nRegistered products:");
            foreach (Product product in _registeredProducts)
                Console.WriteLine($" * " + product);
        }
    }
}