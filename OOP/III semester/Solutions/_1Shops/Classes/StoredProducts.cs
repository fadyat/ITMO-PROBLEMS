using System.Collections.Generic;
using Shops.Exceptions;

namespace Shops.Classes
{
    public class StoredProducts
    {
        private readonly Dictionary<Product, ProductInfo> _storedProducts;

        public StoredProducts()
        {
            _storedProducts = new Dictionary<Product, ProductInfo>();
        }

        private StoredProducts(Dictionary<Product, ProductInfo> products)
        {
            _storedProducts = products;
        }

        public StoredProductsBuilder ToBuilder()
        {
            StoredProductsBuilder newBuilder = new StoredProductsBuilder()
                .WithStoredProducts(_storedProducts);

            return newBuilder;
        }

        public uint? GetPriceForMultipleProducts(Product productToBuy, uint quantity)
        {
            if (_storedProducts[productToBuy].Quantity < quantity)
                return null;

            uint multiplePrice = quantity * _storedProducts[productToBuy].Price;
            return multiplePrice;
        }

        public ProductInfo GetProductInfo(Product product)
        {
            return _storedProducts[product];
        }

        public override string ToString()
        {
            string products = string.Empty;
            foreach ((Product product, ProductInfo productInfo) in _storedProducts)
                products += product + " " + productInfo;

            return products;
        }

        public override int GetHashCode()
        {
            return _storedProducts.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (StoredProducts)obj;
            return other != null && Equals(_storedProducts, other._storedProducts);
        }

        public class StoredProductsBuilder
        {
            private Dictionary<Product, ProductInfo> _products;

            public StoredProductsBuilder()
            {
                _products = new Dictionary<Product, ProductInfo>();
            }

            public StoredProductsBuilder WithStoredProducts(Dictionary<Product, ProductInfo> products)
            {
                _products = products;
                return this;
            }

            public StoredProductsBuilder AddProduct(Product product, ProductInfo productInfo)
            {
                if (!_products.ContainsKey(product))
                {
                    _products.Add(product, new ProductInfo.ProductInfoBuilder()
                        .Build());
                }

                _products[product] = new ProductInfo.ProductInfoBuilder()
                    .WithPrice(productInfo.Price)
                    .WithQuantity(productInfo.Quantity + _products[product].Quantity)
                    .Build();

                return this;
            }

            public StoredProductsBuilder PurchaseProduct(Product product, uint quantity)
            {
                uint previousQuantity = _products[product].Quantity;
                if (previousQuantity < quantity)
                    throw new ShopException("Not enough products!");

                ProductInfo newProductInfo = _products[product].ToBuilder()
                    .WithQuantity(previousQuantity - quantity)
                    .Build();

                _products[product] = newProductInfo;
                return this;
            }

            public StoredProducts Build()
            {
                StoredProducts finallyStoredProducts = new (_products);
                return finallyStoredProducts;
            }
        }
    }
}