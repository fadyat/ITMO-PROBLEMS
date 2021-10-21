using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Classes;
using Shops.Repositories.Interfaces;

namespace Shops.Repositories
{
    public class RegisteredProductRepository : IRegisteredProductRepository
    {
        private readonly List<Product> _registeredProducts;

        public RegisteredProductRepository()
        {
            _registeredProducts = new List<Product>();
        }

        public void Save(Product product)
        {
            _registeredProducts.Add(product);
        }

        public IEnumerable<Product> GetAll()
        {
            return _registeredProducts;
        }

        public Product GetProduct(int id)
        {
            foreach (Product product in _registeredProducts
                .Where(product => product.Id == id))
            {
                return product;
            }

            throw new Exception(); // fix
        }
    }
}