namespace Shops.Classes
{
    public class Product
    {
        public Product(string productName) { Name = productName; }

        public string Name { get; }

        public override int GetHashCode() { return Name.GetHashCode(); }

        public override string ToString() { return Name; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Product)obj;
            return other != null && Name == other.Name;
        }
    }
}