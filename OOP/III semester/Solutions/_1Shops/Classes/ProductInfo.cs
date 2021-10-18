using System;

namespace Shops.Classes
{
    public class ProductInfo
    {
        private ProductInfo(uint price, uint quantity)
        {
            Price = price;
            Quantity = quantity;
        }

        public uint Price { get; }

        public uint Quantity { get; }

        public override string ToString()
        {
            return Quantity + " " + (Price / 100) + "," + (Price % 100) + '\n';
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Price, Quantity);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (ProductInfo)obj;
            return other != null && Equals(Quantity, other.Quantity);
        }

        public ProductInfoBuilder ToBuilder()
        {
            ProductInfoBuilder productInfoBuilder = new ProductInfoBuilder()
                .WithPrice(Price)
                .WithQuantity(Quantity);

            return productInfoBuilder;
        }

        public class ProductInfoBuilder
        {
            private uint _price;
            private uint _quantity;

            public ProductInfoBuilder WithPrice(uint price)
            {
                _price = price;
                return this;
            }

            public ProductInfoBuilder WithQuantity(uint quantity)
            {
                _quantity = quantity;
                return this;
            }

            public ProductInfo Build()
            {
                ProductInfo finallyProductInfo = new (_price, _quantity);
                return finallyProductInfo;
            }
        }
    }
}