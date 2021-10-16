using System;
using System.Linq;
using IsuExtra.Exceptions;

namespace IsuExtra.Classes.Standard
{
    /// <summary> Correct CourseNumbers. </summary>
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
        private readonly string _groupNumber;

        public GroupName(uint courseNumber, string groupNumber, string facultyTag = "M3")
        {
            if (!Enum.IsDefined(typeof(CorrectCourses), courseNumber))
                throw new IsuException("Wrong course number format!");

            if (groupNumber.Length != 2 || groupNumber.Any(symbol => symbol is < '0' or > '9'))
                throw new IsuException("Group number should have two digits!");

            if (facultyTag.First() is < 'A' or > 'Z')
                throw new IsuException("Faculty tag should begin with letter!");

            (Course, _groupNumber, FacultyTag) = (courseNumber, groupNumber, facultyTag);
        }

        public uint Course { get; }

        public string FacultyTag { get; }

        public override int GetHashCode() { return HashCode.Combine(Course, _groupNumber); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (GroupName)obj;
            return other != null && Course == other.Course &&
                   _groupNumber == other._groupNumber && FacultyTag == other.FacultyTag;
        }

        public override string ToString() { return FacultyTag + Course + _groupNumber; }
    }
}