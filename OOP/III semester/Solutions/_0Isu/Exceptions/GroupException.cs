namespace Isu.Exceptions
{
    public class GroupException : IsuException
    {
        public GroupException(string message)
            : base(message) { }
    }
}