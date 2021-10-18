using System.Collections.Generic;

namespace Shops.Classes
{
    public class StoredProducts
    {
        private readonly Dictionary<Product, ProductInfo> _storedProducts;

        private StoredProducts() { }
        private StoredProducts(Dictionary<Product, ProductInfo> products)
        {
            _storedProducts = products;
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

        public ProductInfo GetProductInfo(Product product)
        {
            return _storedProducts[product];
        }

        public StoredProductsBuilder ToBuilder()
        {
            StoredProductsBuilder newBuilder = new StoredProductsBuilder()
                .WithStoredProducts(_storedProducts);

            return newBuilder;
        }

        public int FakeBuy(Product productToBuy, uint quantity)
        {
            if (_storedProducts[productToBuy].Quantity < quantity)
                return -1;

            int fakeSpendingMoney = (int)(quantity * _storedProducts[productToBuy].Price);
            return fakeSpendingMoney;
        }

        public class StoredProductsBuilder
        {
            private Dictionary<Product, ProductInfo> _products;

            public StoredProductsBuilder()
            {
                _products = new Dictionary<Product, ProductInfo>();
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

            public StoredProductsBuilder WithStoredProducts(Dictionary<Product, ProductInfo> products)
            {
                _products = products;
                return this;
            }

            public StoredProductsBuilder PurchaseProduct(Product product, uint quantity)
            {
                uint previousQuantity = _products[product].Quantity;
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