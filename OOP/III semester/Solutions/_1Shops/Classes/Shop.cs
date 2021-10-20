using System;

namespace Shops.Classes
{
    public class Shop
    {
        private readonly string _name;
        private readonly int _id;

        private Shop(string name, int id)
        {
            _name = name;
            _id = id;
        }

        public ShopBuilder ToBuilder()
        {
            ShopBuilder shopBuilder = new ShopBuilder()
                .WithName(_name)
                .WithId(_id);

            return shopBuilder;
        }

        public override bool Equals(object obj)
        {
            if (obj is not Shop item)
                return false;

            return Equals(_name, item._name)
                   && Equals(_id, item._id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_id, _name);
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