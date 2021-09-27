namespace Shops.Classes
{
    public class Product
    {
        private readonly string _name;

        public Product(string productName) { _name = productName; }

        public string Name => _name;

        public static bool operator ==(Product a, Product b) { return Equals(a, b); }

        public static bool operator !=(Product a, Product b) { return !Equals(a, b); }

        public override int GetHashCode() { return _name.GetHashCode(); }

        public override string ToString() { return _name; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Product)obj;
            return other != null && _name == other._name;
        }
    }
}