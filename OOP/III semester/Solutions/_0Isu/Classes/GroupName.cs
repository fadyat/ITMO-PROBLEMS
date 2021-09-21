using System;
using System.Linq;
using Isu.Exceptions;

namespace Isu.Classes
{
    public class GroupName
    {
        private readonly string _tag;
        private readonly uint _course;
        private readonly string _groupNumber;

        public GroupName(uint courseNumber, string groupNumber, string tag = "M3")
        {
            if (!Enum.IsDefined(typeof(CorrectCourses), courseNumber))
                throw new IsuException("Wrong course number format!");

            if (groupNumber.Length != 2 || groupNumber.Any(symbol => symbol < '0' || symbol > '9'))
                throw new IsuException("Group number should have two digits!");

            (_course, _groupNumber, _tag) = (courseNumber, groupNumber, tag);
        }

        private enum CorrectCourses : uint
        {
            First = 1,
            Second,
            Third,
            Fours,
        }

        public uint Course => _course;

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
    }
}