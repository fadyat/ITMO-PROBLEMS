using System;

namespace Shops.Exceptions
{
    public class ShopManagerException : Exception
    {
        public ShopManagerException(string message)
            : base(message) { }
    }
}