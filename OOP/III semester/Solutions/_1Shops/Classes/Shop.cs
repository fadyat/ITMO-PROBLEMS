using System.Collections.Generic;
using System.Xml.XPath;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class Shop : IShop
    {
        private readonly uint _id;
        private readonly string _name;
        private readonly string _address;
        private readonly Dictionary<Product, ProductQuantity> _storedProducts;

        public Shop() { }
        public Shop(uint shopId, string shopName, string shopAddress)
        {
            (_id, _name, _address) = (shopId, shopName, shopAddress);
            _storedProducts = new Dictionary<Product, ProductQuantity>();
        }

        public uint Id => _id;
        public string Name => _name;
        public string Address => _address;
        public Dictionary<Product, ProductQuantity> StoredProducts => _storedProducts;
    }
}