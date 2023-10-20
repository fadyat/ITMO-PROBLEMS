using System;

namespace Shops.Classes
{
    public class Shop
    {
        private readonly string _name;

        private Shop(string name, int id)
        {
            _name = name;
            Id = id;
        }

        public int Id { get; }

        public ShopBuilder ToBuilder()
        {
            ShopBuilder shopBuilder = new ShopBuilder()
                .WithName(_name)
                .WithId(Id);

            return shopBuilder;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Shop item)
                return false;

            return Equals(_name, item._name)
                   && Equals(Id, item.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, _name);
        }

        public override string ToString()
        {
            return $"{_name} {Id}";
        }

        public class ShopBuilder
        {
            private string _name;
            private int _id;

            public ShopBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public ShopBuilder WithId(int id)
            {
                _id = id;
                return this;
            }

            public Shop Build()
            {
                var finallyShop = new Shop(_name, _id);
                return finallyShop;
            }
        }
    }
}