using System.Collections.Generic;

namespace Shops.Classes
{
    public class Shop
    {
        private readonly uint _id;
        private readonly string _name;
        private readonly string _address;
        private readonly Dictionary<Product, ProductInfo> _storedProducts;

        public Shop() { }
        public Shop(uint shopId, string shopName, string shopAddress)
        {
            (_id, _name, _address) = (shopId, shopName, shopAddress);
            _storedProducts = new Dictionary<Product, ProductInfo>();
        }

        public string Name => _name;
        public Dictionary<Product, ProductInfo> StoredProducts => _storedProducts;

        public static bool operator ==(Shop a, Shop b) { return Equals(a, b); }

        public static bool operator !=(Shop a, Shop b) { return !Equals(a, b); }

        public override int GetHashCode() { return _id.GetHashCode(); }

        public override string ToString() { return _name + " " + _address + " " + _id; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Shop)obj;
            return other != null && _id == other._id;
        }
    }
}