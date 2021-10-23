using System;

namespace Shops.Exceptions
{
    public class ShopException : Exception
    {
        public ShopException(string message)
            : base(message) { }
    }
}