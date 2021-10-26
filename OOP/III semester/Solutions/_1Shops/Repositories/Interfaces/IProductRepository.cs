using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Repositories.Interfaces
{
    public interface IProductRepository
    {
        void Save(Product product);

        IEnumerable<Product> GetAll();

        Product GetProduct(int id, int shopId);

        Product FindProduct(int id, int shopId);

        void Delete(int id, int shopId);

        void Print();

        void CheckProduct(int id);
    }
}