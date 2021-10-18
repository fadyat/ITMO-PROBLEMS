using System.Collections.Generic;
using Shops.Exceptions;

namespace Shops.Classes
{
    public class Shop
    {
        private Shop() { }
        private Shop(uint shopId, string shopName, StoredProducts storedProducts)
        {
            (Id, Name) = (shopId, shopName);
            StoredProducts = storedProducts;
        }

        public StoredProducts StoredProducts { get; }

        public string Name { get; }

        public uint Id { get; }

        public override string ToString()
        {
            return " \t # " + Name + " " + Id + '\n' + StoredProducts;
        }

        public override int GetHashCode() { return Id.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Shop)obj;
            return other != null && Equals(Id, other.Id);
        }

        public ShopBuilder ToBuilder()
        {
            ShopBuilder shopBuilder = new ();
            shopBuilder.WithId(Id);
            shopBuilder.WithName(Name);
            shopBuilder.WithProducts(StoredProducts);
            return shopBuilder;
        }

        public class ShopBuilder
        {
            private StoredProducts _products;
            private uint _id;
            private string _name;

            public ShopBuilder()
            {
                _products = new StoredProducts.StoredProductsBuilder()
                    .Build();
            }

            public ShopBuilder WithProducts(StoredProducts storedProducts)
            {
                _products = storedProducts;
                return this;
            }

            public ShopBuilder WithId(uint shopId)
            {
                _id = shopId;
                return this;
            }

            public ShopBuilder WithName(string shopName)
            {
                _name = shopName;
                return this;
            }

            public ShopBuilder AddProduct(Product productToAdd, ProductInfo productInfo)
            {
                _products = _products.ToBuilder()
                    .AddProduct(productToAdd, productInfo)
                    .Build();
                return this;
            }

            public ShopBuilder AddProducts(List<Product> products, List<ProductInfo> productInfos)
            {
                for (int i = 0; i < products.Count; i++)
                {
                    _products = _products.ToBuilder()
                        .AddProduct(products[i], productInfos[i])
                        .Build();
                }

                return this;
            }

            public ShopBuilder PurchaseProduct(Product productToPurchase, uint quantity)
            {
                _products = _products.ToBuilder()
                    .PurchaseProduct(productToPurchase, quantity)
                    .Build();

                return this;
            }

            public Shop Build()
            {
                Shop finallyShop = new (_id, _name, _products);
                return finallyShop;
            }
        }
    }
}