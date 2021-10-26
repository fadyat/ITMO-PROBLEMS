using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Classes;
using Shops.Exceptions;
using Shops.Repositories.Interfaces;

namespace Shops.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public ProductRepository()
        {
            _products = new List<Product>();
        }

        public void Save(Product product)
        {
            _products.Add(product);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetProduct(int id, int shopId)
        {
            foreach (Product product in _products
                .Where(product => Equals(product.Id, id) &&
                                  Equals(product.ShopId, shopId)))
            {
                return product;
            }

            throw new ProductException("No such product!");
        }

        public Product FindProduct(int id, int shopId)
        {
            return _products.FirstOrDefault(product => Equals(product.Id, id) &&
                                                       Equals(product.ShopId, shopId));
        }

        public void Delete(int id, int shopId)
        {
            foreach (Product product in _products
                .Where(product => Equals(product.Id, id) &&
                                  Equals(product.ShopId, shopId)))
            {
                _products.Remove(product);
                break;
            }
        }

        public void CheckProduct(int id)
        {
            if (_products.Any(product => Equals(product.Id, id)))
            {
                return;
            }

            throw new ProductException("No such product!");
        }

        public void Print()
        {
            foreach (Product product in _products)
            {
                Console.WriteLine(product);
            }
        }
    }
}