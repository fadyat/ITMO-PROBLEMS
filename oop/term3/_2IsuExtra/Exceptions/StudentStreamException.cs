using System;

namespace IsuExtra.Exceptions
{
    public class StudentStreamException : Exception
    {
        public StudentStreamException(string message)
            : base(message) { }
    }
}