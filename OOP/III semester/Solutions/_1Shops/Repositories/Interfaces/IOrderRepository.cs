using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order shop);

        IEnumerable<Order> GetAll();

        Order GetOrder(int id);
    }
}