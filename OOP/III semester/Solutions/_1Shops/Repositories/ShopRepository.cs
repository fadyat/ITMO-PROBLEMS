using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Classes;
using Shops.Exceptions;
using Shops.Repositories.Interfaces;

namespace Shops.Repositories
{
    public class ShopRepository : IShopRepository
    {
        private readonly List<Shop> _shops;

        public ShopRepository()
        {
            _shops = new List<Shop>();
        }

        public void Save(Shop shop)
        {
            _shops.Add(shop);
        }

        public IEnumerable<Shop> GetAll()
        {
            return _shops;
        }

        public Shop GetShop(int shopId)
        {
            foreach (Shop shop in _shops
                .Where(shop => Equals(shop.Id, shopId)))
            {
                return shop;
            }

            throw new ShopException("No such shop!");
        }

        public void Print()
        {
            foreach (Shop shop in _shops)
            {
                Console.WriteLine(shop);
            }
        }
    }
}