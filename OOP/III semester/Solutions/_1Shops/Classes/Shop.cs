using System.Collections.Generic;

namespace Shops.Classes
{
    public class Shop
    {
        private readonly string _address;

        public Shop() { }
        public Shop(uint shopId, string shopName, string shopAddress)
        {
            (Id, Name, _address) = (shopId, shopName, shopAddress);
            StoredProducts = new Dictionary<Product, ProductInfo>();
            RegisteredProducts = new HashSet<Product>();
        }

        public string Name { get; }
        public uint Id { get; }

        public Dictionary<Product, ProductInfo> StoredProducts { get; }
        public HashSet<Product> RegisteredProducts { get; }

        public override string ToString() { return Name + " " + _address + " " + Id; }
    }
}