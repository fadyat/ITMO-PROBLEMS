using System;

namespace IsuExtra.Exceptions
{
    public class IsuException : Exception
    {
        public IsuException(string message)
            : base(message) { }
    }
}