using System;

namespace Banks.Exceptions
{
    public class BankException : Exception
    {
        public BankException(string message)
            : base(message)
        {
        }
    }
}