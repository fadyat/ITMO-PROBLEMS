using System;

namespace Shops.Classes
{
    public class Product
    {
        private readonly string _name;
        private readonly int _id;
        private readonly int _shopId;
        private readonly int _price;
        private readonly int _quantity;

        private Product(string name, int id, int shopId, int price, int quantity)
        {
            _name = name;
            _id = id;
            _shopId = shopId;
            _price = price;
            _quantity = quantity;
        }

        public ProductBuilder ToBuilder()
        {
            ProductBuilder productBuilder = new ProductBuilder()
                .WithName(_name)
                .WithId(_id)
                .WithShopId(_shopId)
                .WithPrice(_price)
                .WithQuantity(_quantity);

            return productBuilder;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Product item)
                return false;

            return Equals(_name, item._name)
                   && Equals(_id, item._id)
                   && Equals(_shopId, item._shopId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _name);
        }

        public class ProductBuilder
        {
            private string _name;
            private int _id;
            private int _shopId;
            private int _price;
            private int _quantity;

            public ProductBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ProductBuilder WithId(int id)
            {
                _id = id;
                return this;
            }

            public ProductBuilder WithShopId(int shopId)
            {
                _shopId = shopId;
                return this;
            }

            public ProductBuilder WithPrice(int price)
            {
                _price = price;
                return this;
            }

            public ProductBuilder WithQuantity(int quantity)
            {
                _quantity = quantity;
                return this;
            }

            public Product Build()
            {
                var finallyProduct = new Product(_name, _id, _shopId, _price, _quantity);
                return finallyProduct;
            }
        }
    }
}