using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Repositories.Interfaces
{
    public interface ISupplyRepository
    {
        void Save(Supply shop);

        IEnumerable<Supply> GetAll();

        Supply GetSupply(int id);

        void Print();
    }
}