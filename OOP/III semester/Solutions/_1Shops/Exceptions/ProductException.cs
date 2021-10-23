using System;

namespace Shops.Exceptions
{
    public class ProductException : Exception
    {
        public ProductException(string message)
            : base(message) { }
    }
}