using System.Collections.Generic;

namespace Shops.Classes
{
    public class Order
    {
        public Order(List<OrderItem> newOrders, int id)
        {
            Orders = newOrders;
            Id = id;
        }

        public List<OrderItem> Orders { get; }

        public int Id { get; }
    }
}