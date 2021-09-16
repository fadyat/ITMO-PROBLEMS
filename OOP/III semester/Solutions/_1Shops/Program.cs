using System;
using Shops.Classes;

namespace Shops
{
    internal class Program
    {
        private static void Main()
        {
            /*

             ShopManager ---- List<Shop> _shops + List<Product> _registeredProducts
             |
             |_ (CreateShop, RegisterProduct, CheapProductSearch, DeliverProducts, SetProductsPrices)
                 |            |
                 |            |
                 |            |_ Product ---- _price + _name
                 |
                 |_ Shop ---- _id + _name + _address + Dictionary<Product, ProductQuantity> _storedProducts


             Customer ---- _name + _money + Dictionary<Product, ProductQuantity> _purchasedProducts
             |
             |_ (PurchaseProduct)

             */
        }
    }
}
