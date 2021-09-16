using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShop
    {
        uint Id { get; }
        string Name { get; }
        string Address { get; }
        Dictionary<Product, ProductQuantity> StoredProducts { get; }
    }
}