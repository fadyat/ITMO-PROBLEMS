using System.Collections.Generic;
using Shops.Classes;
using Shops.Interfaces;

namespace Shops.Services
{
    public class ShopService : IShopService
    {
        private List<Shop> _shops;
        private int _issuedId;

        public ShopService()
        {
            _shops = new List<Shop>();
            _issuedId = 100000;
        }

        public Shop AddShop(string shopName)
        {
            Shop newShop = new Shop.ShopBuilder()
                .WithName(shopName)
                .WithId(_issuedId++)
                .Build();

            _shops.Add(newShop);
            return newShop;
        }
    }
}