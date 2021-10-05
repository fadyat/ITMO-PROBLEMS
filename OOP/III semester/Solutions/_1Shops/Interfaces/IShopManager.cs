using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        Shop CreateShop(string shopName);
        Shop CheapProductSearch(List<(Product, uint cheapCnt)> productsToBuyCheap);
    }
}