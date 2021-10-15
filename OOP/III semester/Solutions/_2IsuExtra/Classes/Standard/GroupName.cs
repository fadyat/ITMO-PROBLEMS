using System;
using System.Linq;
using IsuExtra.Exceptions;

namespace IsuExtra.Classes.Standard
{
    public class GroupName
    {
        private readonly string _tag;
        private readonly string _groupNumber;

        public GroupName(uint courseNumber, string groupNumber, string tag = "M3")
        {
            if (!Enum.IsDefined(typeof(CorrectCourses), courseNumber))
                throw new IsuException("Wrong course number format!");

            if (groupNumber.Length != 2 || groupNumber.Any(symbol => symbol < '0' || symbol > '9'))
                throw new IsuException("Group number should have two digits!");

            (Course, _groupNumber, _tag) = (courseNumber, groupNumber, tag);
        }

        private enum CorrectCourses : uint
        {
            First = 1,
            Second,
            Third,
            Fours,
        }

        public uint Course { get; }
        public override int GetHashCode() { return HashCode.Combine(Course, _groupNumber); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (GroupName)obj;
            return other != null && Course == other.Course &&
                   _groupNumber == other._groupNumber && _tag == other._tag;
        }

        public override string ToString() { return _tag + Course + _groupNumber; }
    }
}