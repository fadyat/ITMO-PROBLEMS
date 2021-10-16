using System;

namespace IsuExtra.Exceptions
{
    public class IsuExtraException : Exception
    {
        public IsuExtraException(string message)
            : base(message) { }
    }
}