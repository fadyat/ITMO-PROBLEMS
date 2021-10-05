using System;
using System.Collections.Generic;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class ShopManager : IShopManager
    {
        private HashSet<Shop> _createdShops;
        private uint _spareId;

        public ShopManager()
        {
            _createdShops = new HashSet<Shop>();
            _spareId = 100000;
        }

        public Shop CreateShop(string shopName)
        {
            var newShop = new Shop(_spareId++, shopName);
            _createdShops = new HashSet<Shop>(_createdShops) { newShop };
            return newShop;
        }

        public Shop CheapProductSearch(List<(Product, uint cheapCnt)> productsToBuyCheap)
        {
            double lowestPrice = 1e9;
            Shop shopWithLowestPrice = null;
            foreach (Shop shop in _createdShops)
            {
                double currentPrice = 0;
                bool correctShop = true;
                foreach ((Product product, uint cheapCnt) in productsToBuyCheap)
                {
                    if (!shop.RegisteredProducts.Contains(product) || !shop.StoredProducts.ContainsKey(product) ||
                        shop.StoredProducts[product].Cnt < cheapCnt)
                    {
                        correctShop = false;
                        break;
                    }

                    currentPrice += cheapCnt * shop.StoredProducts[product].Price;
                }

                if (!correctShop || currentPrice >= lowestPrice) continue;
                lowestPrice = currentPrice;
                shopWithLowestPrice = shop;
            }

            return shopWithLowestPrice;
        }

        public void Info()
        {
            Console.WriteLine("\nCreated shops:");
            foreach (Shop shop in _createdShops)
            {
                Console.WriteLine(" # " + shop);
                Console.WriteLine(" - Registered products:");
                foreach (Product product in shop.RegisteredProducts)
                    Console.WriteLine($"\t * " + product);
                Console.WriteLine(" - Stored products:");
                foreach ((Product product, ProductInfo productInfo) in shop.StoredProducts)
                {
                    Console.WriteLine("\t * " + product + " " + productInfo);
                }

                Console.WriteLine();
            }
        }
    }
}