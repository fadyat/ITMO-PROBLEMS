namespace Shops.Classes
{
    public class Product
    {
        private Product() { }
        private Product(string productName, uint id)
        {
            Name = productName;
            Id = id;
        }

        public string Name { get; }

        public uint Id { get; }

        public override int GetHashCode() { return Name.GetHashCode(); }

        public override string ToString() { return "\t\t * " + Name + " " + Id; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Product)obj;
            return other != null && Equals(Name, other.Name) && Equals(Id, other.Id);
        }

        public ProductBuilder ToBuilder()
        {
            ProductBuilder productBuilder = new ();
            productBuilder.WithName(Name)
                .WithId(Id);
            return productBuilder;
        }

        public class ProductBuilder
        {
            private string _name;
            private uint _id;

            public ProductBuilder WithName(string productName)
            {
                _name = productName;
                return this;
            }

            public ProductBuilder WithId(uint productId)
            {
                _id = productId;
                return this;
            }

            public Product Build()
            {
                Product finallyProduct = new (_name, _id);
                return finallyProduct;
            }
        }
    }
}