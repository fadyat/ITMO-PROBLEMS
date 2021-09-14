using System;
using Isu.Tools;

namespace Isu.Services
{
    public class CourseNumber
    {
        private readonly byte _course;

        public CourseNumber(byte number)
        {
            if (_course > 0 && _course < 10)
                throw new CourseNumberException("Сourse number must be digit not equal to zero!");

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

        public override string ToString() { return Convert.ToString(_course); }
    }
}