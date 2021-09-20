using Isu.Exceptions;

namespace Isu.Classes
{
    public class CourseNumber
    {
        private readonly byte _course;

        public CourseNumber(byte number)
        {
            if (number <= 0 || number > 9)
                throw new IsuException("Course number must be digit not equal to zero!");

            _course = number;
        }

        public static bool operator ==(CourseNumber a, CourseNumber b) { return Equals(a, b); }

        public static bool operator !=(CourseNumber a, CourseNumber b) { return !Equals(a, b); }

        public override int GetHashCode() { return _course.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (CourseNumber)obj;
            return other != null && _course == other._course;
        }

        public override string ToString() { return _course.ToString(); }
    }
}