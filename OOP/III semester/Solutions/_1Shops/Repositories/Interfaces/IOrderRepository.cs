using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order order);

        IEnumerable<Order> GetAll();

        Order GetOrder(int id);

        void Print();
    }
}