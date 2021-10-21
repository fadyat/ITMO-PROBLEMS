using System;
using System.Collections.Generic;
using Shops.Classes;
using Shops.Repositories;
using Shops.Services.Interfaces;

namespace Shops.Services
{
    public class ShopService : IShopService
    {
        private readonly ShopRepository _shopRepository;
        private int _issuedShopId;

        public ShopService()
        {
            _shopRepository = new ShopRepository();
            _issuedShopId = 100000;
        }

        public IEnumerable<Shop> Shops => _shopRepository.GetAll();

        public Shop CreateShop(string name)
        {
            Shop newShop = new Shop.ShopBuilder()
                .WithName(name)
                .WithId(_issuedShopId++)
                .Build();

            _shopRepository.Save(newShop);
            return newShop;
        }

        public void Print()
        {
            Console.WriteLine(" # Shops:");
            foreach (Shop shop in _shopRepository.GetAll())
                Console.WriteLine($"\t{shop}");
        }
    }
}