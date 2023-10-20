using System;

namespace Isu.Exceptions
{
    public class IsuException : Exception
    {
        public IsuException(string message)
            : base(message) { }
    }
}