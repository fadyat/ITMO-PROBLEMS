using System;
using System.Collections.Generic;
using Shops.Exceptions;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class ShopManager : IShopManager
    {
        private readonly HashSet<Shop> _createdShops; // shops ids
        private readonly HashSet<Product> _registeredProducts; // products names
        private uint _spareId;

        public ShopManager()
        {
            _createdShops = new HashSet<Shop>();
            _registeredProducts = new HashSet<Product>();
            _spareId = 100000;
        }

        public uint Id => _spareId;
        public HashSet<Shop> CreatedShops => _createdShops;
        public HashSet<Product> RegisteredProducts => _registeredProducts;

        public Shop CreateShop(string shopName, string shopAddress)
        {
            var newShop = new Shop(_spareId++, shopName, shopAddress);
            _createdShops.Add(newShop);
            return newShop;
        }

        public Product RegisterProduct(string productName)
        {
            var newProduct = new Product(productName);
            _registeredProducts.Add(newProduct);
            return newProduct;
        }

        public Shop CheapProductSearch(List<KeyValuePair<Product, ProductQuantity>> productsToBuyCheap)
        {
            double lowestPrice = 1e9;
            var shopWithLowestPrice = new Shop();
            foreach (Shop shop in _createdShops)
            {
                double currentPrice = 0;
                bool correctShop = true;
                var alreadyUsed = new Dictionary<Product, uint>();

                foreach ((Product product, ProductQuantity productQuantity) in productsToBuyCheap)
                {
                    if (!_registeredProducts.Contains(product))
                        return null;

                    if (shop.StoredProducts[product].Quantity - alreadyUsed[product] < productQuantity.Quantity)
                    {
                        correctShop = false;
                        break;
                    }

                    alreadyUsed[product] += productQuantity.Quantity;
                    currentPrice += product.Price * productQuantity.Quantity;
                }

                alreadyUsed.Clear();
                if (!correctShop || currentPrice >= lowestPrice) continue;

                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void AddProducts(Shop shop, List<KeyValuePair<Product, ProductQuantity>> products, List<double> productsPrices)
        {
            if (!_createdShops.Contains(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            if (productsPrices.Count != products.Count)
                throw new ShopManagerException($"Can't set prices, not enough data!");

            int i = 0;
            foreach ((Product product, ProductQuantity productQuantity) in products)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ProductException($"Product {product.Name} hasn't been registered!");

                if (!shop.StoredProducts.ContainsKey(product))
                    shop.StoredProducts.Add(product, new ProductQuantity(0));

                shop.StoredProducts[product].Quantity += productQuantity.Quantity;
                product.Price = productsPrices[i++];
            }
        }

        public void PurchaseProduct(Customer customer, Shop shop, List<KeyValuePair<Product, ProductQuantity>> productsToPurchase)
        {
            if (!_createdShops.Contains(shop))
                throw new ShopException($"Shop {shop.Name} hasn't been created!");

            foreach ((Product product, ProductQuantity productQuantity) in productsToPurchase)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ProductException($"Product {product.Name} hasn't been registered!");

                if (shop.StoredProducts[product].Quantity >= productQuantity.Quantity)
                {
                    if (customer.Money >= product.Price * productQuantity.Quantity)
                    {
                        shop.StoredProducts[product].Quantity -= productQuantity.Quantity;
                        customer.Money -= product.Price * productQuantity.Quantity;
                    }
                    else
                    {
                        throw new CustomerException(
                            $"Not enough money need more: {(product.Price * productQuantity.Quantity) - customer.Money}");
                    }
                }
                else
                {
                    throw new ShopException(
                        $"Not enough product need more: {productQuantity.Quantity - shop.StoredProducts[product].Quantity}");
                }
            }
        }

        public void LookThrow()
        {
            Console.WriteLine("\n\nCreated shops:");
            foreach (Shop shop in _createdShops)
            {
                Console.WriteLine($" * " + shop);
                foreach ((Product product, ProductQuantity quantity) in shop.StoredProducts)
                {
                    Console.WriteLine("\t - " + product + " " + quantity);
                }
            }

            Console.WriteLine("\n\nRegistered products:");
            foreach (Product product in _registeredProducts)
            {
                Console.WriteLine($" * " + product.Name);
            }
        }
    }
}