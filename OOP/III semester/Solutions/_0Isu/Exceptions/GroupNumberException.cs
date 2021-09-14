namespace Isu.Exceptions
{
    public class GroupNumberException : GroupNameException
    {
        public GroupNumberException(string message)
            : base(message) { }
    }
}