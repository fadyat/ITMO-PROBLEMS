namespace Isu.Exceptions
{
    public class CourseNumberException : GroupNameException
    {
        public CourseNumberException(string message)
            : base(message) { }
    }
}