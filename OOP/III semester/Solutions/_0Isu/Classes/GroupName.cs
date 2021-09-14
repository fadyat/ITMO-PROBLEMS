using System;
using Isu.Exceptions;

namespace Isu.Classes
{
    public class GroupName
    {
        private const string Tag = "M3";
        private readonly CourseNumber _course;
        private readonly string _groupNumber;

        public GroupName(CourseNumber course, string group)
        {
            if (group.Length != 2)
                throw new GroupNumberException("Group number must be two numbers!");

            if (!((group[0] >= '0' && group[0] <= '9') && (group[1] >= '0' && group[1] <= '9')))
                throw new GroupNumberException("Group number should have only digits!");

            (_course, _groupNumber) = (course, group);
        }

        public CourseNumber Course { get => _course; }

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

        public override string ToString() { return Tag + Convert.ToString(_course) + Convert.ToString(_groupNumber); }
    }
}