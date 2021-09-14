namespace Isu.Exceptions
{
    public class StudentException : GroupException
    {
        public StudentException(string message)
            : base(message) { }
    }
}