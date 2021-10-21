using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Repositories.Interfaces
{
    public interface IRegisteredProductRepository
    {
        void Save(Product shop);

        IEnumerable<Product> GetAll();

        Product GetProduct(int id);
    }
}