using System.Collections.Generic;
using System.Linq;
using Shops.Exceptions;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class Shop : IShop
    {
        private Dictionary<Product, ProductInfo> _storedProducts;
        private HashSet<Product> _registeredProducts;
        public Shop(uint shopId, string shopName)
        {
            (Id, Name) = (shopId, shopName);
            _storedProducts = new Dictionary<Product, ProductInfo>();
            _registeredProducts = new HashSet<Product>();
        }

        public IReadOnlyDictionary<Product, ProductInfo> StoredProducts => _storedProducts;
        public IEnumerable<Product> RegisteredProducts => _registeredProducts;

        public string Name { get; }

        private uint Id { get; }

        public void RegisterProduct(string productName)
        {
            RegisterProduct(new List<string> { productName });
        }

        public void RegisterProduct(IEnumerable<string> productsNames)
        {
            foreach (Product newProduct in productsNames.Select(product => new Product(product)))
            {
                if (_registeredProducts.Contains(newProduct))
                    throw new ShopException($"Product {newProduct.Name} is already registered!");

                _registeredProducts = new HashSet<Product>(_registeredProducts) { newProduct };
            }
        }

        public void AddProducts(Product product, ProductInfo productInfo)
        {
            AddProducts(new List<(Product, ProductInfo)> { (product, productInfo) });
        }

        public void AddProducts(List<(Product, ProductInfo)> products)
        {
            foreach ((Product product, ProductInfo productInfo) in products)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                var newProductInfo = new ProductInfo(productInfo.Cnt, productInfo.Price);
                if (_storedProducts.ContainsKey(product))
                    newProductInfo = new ProductInfo(StoredProducts[product].Cnt + productInfo.Cnt, productInfo.Price);

                _storedProducts.Remove(product);
                _storedProducts = new Dictionary<Product, ProductInfo>(_storedProducts) { { product, newProductInfo } };
            }
        }

        public void PurchaseProduct(ref Customer customer, List<(Product, uint purchaseCnt)> productsToPurchase)
        {
            foreach ((Product product, uint purchaseCnt) in productsToPurchase)
            {
                if (!_registeredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (!_storedProducts.ContainsKey(product))
                    throw new ShopException($"Product {product.Name} hasn't been added in the store! ");

                if (_storedProducts[product].Cnt >= purchaseCnt)
                {
                    if (customer.Money >= _storedProducts[product].Price * purchaseCnt)
                    {
                        var productInfo = new ProductInfo(_storedProducts[product].Cnt - purchaseCnt, _storedProducts[product].Price);
                        _storedProducts.Remove(product);
                        _storedProducts = new Dictionary<Product, ProductInfo>(_storedProducts) { { product, productInfo } };
                        customer = new Customer(customer.Name, customer.Money - (_storedProducts[product].Price * purchaseCnt));
                    }
                    else
                    {
                        throw new ShopException("Not enough money need more!");
                    }
                }
                else
                {
                    throw new ShopException("Not enough products need more!");
                }
            }
        }

        public override string ToString() { return Name + " " + Id; }

        public override int GetHashCode() { return Id.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Shop)obj;
            return other != null && Id == other.Id;
        }
    }
}