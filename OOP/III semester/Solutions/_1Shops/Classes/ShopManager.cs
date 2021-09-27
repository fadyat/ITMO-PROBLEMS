using System;
using System.Collections.Generic;
using Shops.Exceptions;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class ShopManager : IShopManager
    {
        private readonly Dictionary<Shop, Dictionary<Product, (uint have, double price)>> _createdShops;
        private readonly HashSet<Product> _registeredProducts;
        private uint _spareId;

        public ShopManager()
        {
            _createdShops = new Dictionary<Shop, Dictionary<Product, (uint have, double price)>>();
            _registeredProducts = new HashSet<Product>();
            _spareId = 100000;
        }

        public Shop CreateShop(string shopName, string shopAddress)
        {
            var newShop = new Shop(_spareId++, shopName, shopAddress);
            _createdShops.Add(newShop, new Dictionary<Product, (uint have, double price)>());
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

        public Shop CheapProductSearch(List<(Product, uint need)> productsToBuyCheap)
        {
            double lowestPrice = 1e9;
            var shopWithLowestPrice = new Shop();
            foreach ((Shop shop, Dictionary<Product, (uint have, double price)> products) in _createdShops)
            {
                double currentPrice = 0;
                bool correctShop = true;
                foreach ((Product product, uint need) in productsToBuyCheap)
                {
                    if (!_registeredProducts.Contains(product))
                        return null;

                    if (!products.ContainsKey(product) || products[product].have < need)
                    {
                        correctShop = false;
                        break;
                    }

                    currentPrice += need * products[product].price;
                }

                if (!correctShop || currentPrice >= lowestPrice) continue;

                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void AddProducts(Shop shop, List<(Product, uint have, double price)> products)
        {
            if (!_createdShops.ContainsKey(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            foreach ((Product product, uint newCnt, double newPrice) in products)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (!_createdShops[shop].ContainsKey(product))
                    _createdShops[shop].Add(product, (0, 0));

                _createdShops[shop][product] = (_createdShops[shop][product].have + newCnt, newPrice);
            }
        }

        public void PurchaseProduct(Customer customer, Shop shop, List<(Product, uint need)> productsToPurchase)
        {
            if (!_createdShops.ContainsKey(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            foreach ((Product product, uint need) in productsToPurchase)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (_createdShops[shop][product].have >= need)
                {
                    if (customer.Money >= _createdShops[shop][product].price * need)
                    {
                        _createdShops[shop][product] = (_createdShops[shop][product].have - need,
                            _createdShops[shop][product].price);
                        customer.Money -= _createdShops[shop][product].price * need;
                    }
                    else
                    {
                        throw new ShopException($"Not enough money need more!");
                    }
                }
                else
                {
                    throw new ShopException($"Not enough products need more!");
                }
            }
        }

        public void Info()
        {
            Console.WriteLine("\n\nCreated shops:");
            foreach ((Shop shop, Dictionary<Product, (uint have, double price)> products) in _createdShops)
            {
                Console.WriteLine(" * " + shop);
                foreach ((Product product, (uint have, double price)) in products)
                {
                    Console.WriteLine("\t - " + product + " " + have + " " + price);
                }
            }

            Console.WriteLine("\n\nRegistered products:");
            foreach (Product product in _registeredProducts)
                Console.WriteLine($" * " + product);
        }
    }
}