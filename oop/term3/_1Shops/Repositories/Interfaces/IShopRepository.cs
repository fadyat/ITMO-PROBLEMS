using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Repositories.Interfaces
{
    public interface IShopRepository
    {
        void Save(Shop shop);

        IEnumerable<Shop> GetAll();

        Shop GetShop(int shopId);

        void Print();
    }
}