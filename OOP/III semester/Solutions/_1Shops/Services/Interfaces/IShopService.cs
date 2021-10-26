using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Services.Interfaces
{
    public interface IShopService
    {
        Shop CreateShop(string name);

        Shop CheapestShopFinding(List<Product> products, List<int> amounts);
    }
}