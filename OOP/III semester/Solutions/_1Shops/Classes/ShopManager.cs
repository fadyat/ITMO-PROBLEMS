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

        /* public uint Id => _spareId;
           public HashSet<Shop> CreatedShops => _createdShops;
           public HashSet<Product> RegisteredProducts => _registeredProducts; */

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

                    if (shop.StoredProducts[product].Quantity < productInfo.Quantity)
                    {
                        correctShop = false;
                        break;
                    }

                    currentPrice += productInfo.Quantity * shop.StoredProducts[product].Price;
                }

                if (!correctShop || currentPrice >= lowestPrice) continue;

                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void AddProducts(Shop shop, List<(Product, ProductInfo)> products, List<double> productsPrices)
        {
            if (!_createdShops.Contains(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            if (productsPrices.Count != products.Count)
                throw new ShopException($"Can't set prices, not enough data!");

            int i = 0;
            foreach ((Product product, ProductInfo productInfo) in products)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (!shop.StoredProducts.ContainsKey(product))
                    shop.StoredProducts.Add(product, new ProductInfo(0, 0));

                shop.StoredProducts[product].Quantity += productInfo.Quantity;
                shop.StoredProducts[product].Price = productsPrices[i++];
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

                if (shop.StoredProducts[product].Quantity >= productInfo.Quantity)
                {
                    if (customer.Money >= shop.StoredProducts[product].Price * productInfo.Quantity)
                    {
                        shop.StoredProducts[product].Quantity -= productInfo.Quantity;
                        customer.Money -= shop.StoredProducts[product].Price * productInfo.Quantity;
                    }
                    else
                    {
                        throw new ShopException(
                            $"Not enough money need more: {(shop.StoredProducts[product].Price * productInfo.Quantity) - customer.Money}");
                    }
                }
                else
                {
                    throw new ShopException(
                        $"Not enough products need more: {productInfo.Quantity - shop.StoredProducts[product].Quantity}");
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