using System.Collections.Generic;
using System.Linq;
using Shops.Exceptions;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class Shop : IShop
    {
        public Shop(uint shopId, string shopName)
        {
            (Id, Name) = (shopId, shopName);
            StoredProducts = new Dictionary<Product, ProductInfo>();
            RegisteredProducts = new HashSet<Product>();
        }

        public Dictionary<Product, ProductInfo> StoredProducts { get; private set; }

        public HashSet<Product> RegisteredProducts { get; private set; }

        public uint Id { get; }

        public string Name { get; }

        public void RegisterProduct(string productName)
        {
            RegisterProduct(new List<string> { productName });
        }

        public void RegisterProduct(IEnumerable<string> productsNames)
        {
            foreach (Product newProduct in productsNames.Select(product => new Product(product)))
            {
                if (RegisteredProducts.Contains(newProduct))
                    throw new ShopException($"Product {newProduct.Name} is already registered!");

                RegisteredProducts = new HashSet<Product>(RegisteredProducts) { newProduct };
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
                if (!RegisteredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                var newProductInfo = new ProductInfo(productInfo.Cnt, productInfo.Price);
                if (StoredProducts.ContainsKey(product))
                    newProductInfo = new ProductInfo(StoredProducts[product].Cnt + productInfo.Cnt, productInfo.Price);

                StoredProducts.Remove(product);
                StoredProducts = new Dictionary<Product, ProductInfo>(StoredProducts) { { product, newProductInfo } };
            }
        }

        public void PurchaseProduct(ref Customer customer, List<(Product, uint purchaseCnt)> productsToPurchase)
        {
            foreach ((Product product, uint purchaseCnt) in productsToPurchase)
            {
                if (!RegisteredProducts.Contains(product))
                    throw new ShopException($"Product {product.Name} hasn't been registered!");

                if (!StoredProducts.ContainsKey(product))
                    throw new ShopException($"Product {product.Name} hasn't been added in the store! ");

                if (StoredProducts[product].Cnt >= purchaseCnt)
                {
                    if (customer.Money >= StoredProducts[product].Price * purchaseCnt)
                    {
                        var productInfo = new ProductInfo(StoredProducts[product].Cnt - purchaseCnt, StoredProducts[product].Price);
                        StoredProducts.Remove(product);
                        StoredProducts = new Dictionary<Product, ProductInfo>(StoredProducts) { { product, productInfo } };
                        customer = new Customer(customer.Name, customer.Money - (StoredProducts[product].Price * purchaseCnt));
                    }
                    else
                    {
                        throw new ShopException("Not enough money need more!");
                    }
                }
                else
                {
                    throw new ShopException($"Not enough products need more!");
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