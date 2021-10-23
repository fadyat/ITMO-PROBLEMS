using System;

namespace Shops.Exceptions
{
    public class OrderException : Exception
    {
        public OrderException(string message)
            : base(message) { }
    }
}