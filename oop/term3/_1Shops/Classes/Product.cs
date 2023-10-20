using System;

namespace Shops.Classes
{
    public class Product
    {
        private readonly string _name;

        private Product(string name, int id, int shopId, int price, int quantity)
        {
            _name = name;
            Id = id;
            ShopId = shopId;
            Price = price;
            Quantity = quantity;
        }

        public int ShopId { get; }

        public int Price { get; }

        public int Quantity { get; }

        public int Id { get; }

        public ProductBuilder ToBuilder()
        {
            ProductBuilder productBuilder = new ProductBuilder()
                .WithName(_name)
                .WithId(Id)
                .WithShopId(ShopId)
                .WithPrice(Price)
                .WithQuantity(Quantity);

            return productBuilder;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Product item)
                return false;

            return Equals(_name, item._name) &&
                   Equals(Id, item.Id) &&
                   Equals(ShopId, item.ShopId);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, _name);
        }

        public override string ToString()
        {
            return $"{_name} {Id} {ShopId} {Price} {Quantity}";
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