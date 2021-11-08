using System;

namespace IsuExtra.Exceptions
{
    public class ElectiveCourseException : Exception
    {
        public ElectiveCourseException(string message)
            : base(message) { }
    }
}
