using System;

namespace Banks.Exceptions
{
    public class ClientException : Exception
    {
        public ClientException(string message)
            : base(message)
        {
        }
    }
}