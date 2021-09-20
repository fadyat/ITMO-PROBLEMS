using System;
using Isu.Exceptions;

namespace Isu.Classes
{
    public class GroupName
    {
        /* private const string Tag = "M3"; */
        private readonly CourseNumber _course;
        private readonly string _groupNumber;

        public GroupName(CourseNumber courseNumber, string groupNumber)
        {
            if (groupNumber.Length != 2)
                throw new IsuException("Group number must be two numbers!");

            if (!((groupNumber[0] >= '0' && groupNumber[0] <= '9')
                  && (groupNumber[1] >= '0' && groupNumber[1] <= '9')))
                throw new IsuException("Group number should have only digits!");

            (_course, _groupNumber) = (courseNumber, groupNumber);
        }

        public CourseNumber Course => _course;

        public static bool operator ==(GroupName a, GroupName b) { return Equals(a, b); }

        public static bool operator !=(GroupName a, GroupName b) { return !Equals(a, b); }
        public override int GetHashCode() { return HashCode.Combine(_course, _groupNumber); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (GroupName)obj;
            return other != null && _course == other._course &&
                   _groupNumber == other._groupNumber;
        }

        public override string ToString() { return "M3" + _course + _groupNumber; }
    }
}