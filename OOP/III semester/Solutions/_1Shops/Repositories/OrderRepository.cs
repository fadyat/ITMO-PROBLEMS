using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Classes;
using Shops.Exceptions;
using Shops.Repositories.Interfaces;

namespace Shops.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly List<Order> _orders;

        public OrderRepository()
        {
            _orders = new List<Order>();
        }

        public void Save(Order order)
        {
            _orders.Add(order);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders;
        }

        public Order GetOrder(int id)
        {
            foreach (Order order in _orders
                .Where(order => Equals(order.Id, id)))
            {
                return order;
            }

            throw new OrderException("No such order!");
        }

        public void Print()
        {
            foreach (Order order in _orders)
            {
                foreach (OrderItem orderItem in order.Orders)
                {
                    Console.WriteLine($"{orderItem}, {order.Id}");
                }
            }
        }
    }
}