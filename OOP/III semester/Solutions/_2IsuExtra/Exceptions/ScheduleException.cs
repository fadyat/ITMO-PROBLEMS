using System;

namespace IsuExtra.Exceptions
{
    public class ScheduleException : Exception
    {
        public ScheduleException(string message)
            : base(message) { }
    }
}