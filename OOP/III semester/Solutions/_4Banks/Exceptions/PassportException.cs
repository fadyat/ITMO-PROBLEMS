using System;

namespace Banks.Exceptions
{
    public class PassportException : Exception
    {
        public PassportException(string message)
            : base(message)
        {
        }
    }
}