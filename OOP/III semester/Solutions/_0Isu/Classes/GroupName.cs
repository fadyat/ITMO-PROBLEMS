using System;
using System.Linq;
using Isu.Exceptions;

namespace Isu.Classes
{
    /// <summary>
    /// Correct CourseNumbers.
    /// </summary>
    public enum CorrectCourses : uint
    {
        /// <summary> First </summary>
        First = 1,

        /// <summary> Second </summary>
        Second,

        /// <summary> Third </summary>
        Third,

        /// <summary> Fours </summary>
        Fours,
    }

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