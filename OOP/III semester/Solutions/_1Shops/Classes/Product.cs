namespace Shops.Classes
{
    public class Product
    {
        private readonly string _name;
        private readonly uint _id;
        private Product(string productName, uint id)
        {
            _name = productName;
            _id = id;
        }

        public override int GetHashCode() { return _name.GetHashCode(); }

        public override string ToString() { return "\t\t * " + _name + " " + _id; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Product)obj;
            return other != null && Equals(_name, other._name) && Equals(_id, other._id);
        }

        public ProductBuilder ToBuilder()
        {
            ProductBuilder productBuilder = new ();
            productBuilder.WithName(_name)
                .WithId(_id);
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