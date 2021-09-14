using System;

namespace Isu.Tools
{
    public class IsuException : Exception
    {
        protected IsuException(string message)
            : base(message) { }
    }
}