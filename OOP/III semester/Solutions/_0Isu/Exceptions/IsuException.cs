using System;

namespace Isu.Exceptions
{
    public class IsuException : Exception
    {
        protected IsuException(string message)
            : base(message) { }
    }
}