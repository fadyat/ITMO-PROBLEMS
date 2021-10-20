using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Services
{
    public class OrderService
    {
        private readonly List<Order> _orders;
        private readonly int _issuedId;

        public OrderService()
        {
            _orders = new List<Order>();
            _issuedId = 100000;
        }

        private OrderService(List<Order> order, int id)
        {
            _orders = order;
            _issuedId = id;
        }

        public OrderServiceBuilder Build()
        {
            OrderServiceBuilder orderServiceBuilder = new OrderServiceBuilder()
                .WithOrders(_orders)
                .WithId(_issuedId);

            return orderServiceBuilder;
        }

        public class OrderServiceBuilder
        {
            private List<Order> _orders;
            private int _issuedId;

            public OrderServiceBuilder()
            {
                _orders = new List<Order>();
                _issuedId = 100000;
            }

            public OrderServiceBuilder AddOrder(Product product, int amount)
            {
                var newOrder = new Order(product, amount, _issuedId++);
                _orders.Add(newOrder);
                return this;
            }

            public OrderServiceBuilder WithOrders(List<Order> orders)
            {
                _orders = orders;
                return this;
            }

            public OrderServiceBuilder WithId(int id)
            {
                _issuedId = id;
                return this;
            }

            public OrderService Build()
            {
                var finallyOrderService = new OrderService(_orders, _issuedId);
                return finallyOrderService;
            }
        }
    }
}