using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface ICustomer
    {
        double Money { get; set; }
        string Name { get; }
        Dictionary<Product, ProductInfo> PurchasedProducts { get; }
    }
}