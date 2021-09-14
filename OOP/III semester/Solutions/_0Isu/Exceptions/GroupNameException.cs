using System;

namespace Isu.Tools
{
    public class GroupNameException : GroupException
    {
        public GroupNameException(string message)
            : base(message) { }
    }
}