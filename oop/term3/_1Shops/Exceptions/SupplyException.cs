using System;

namespace Shops.Exceptions
{
    public class SupplyException : Exception
    {
        public SupplyException(string message)
            : base(message) { }
    }
}