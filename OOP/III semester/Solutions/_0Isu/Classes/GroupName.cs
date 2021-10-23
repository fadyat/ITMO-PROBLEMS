using System;
using System.Linq;
using Isu.Exceptions;

namespace Isu.Classes
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
        private GroupName(string facultyTag, uint courseNumber, string groupNumber)
        {
            if (!Enum.IsDefined(typeof(CorrectCourses), courseNumber))
                throw new IsuException("Wrong course number format!");

            if (groupNumber.Length != 2 || groupNumber.Any(symbol => symbol is < '0' or > '9'))
                throw new IsuException("Group number should have two digits!");

            if (facultyTag.First() is < 'A' or > 'Z')
                throw new IsuException("Faculty tag should begin with letter!");

            (Course, GroupNumber, FacultyTag) = (courseNumber, groupNumber, facultyTag);
        }

        public uint Course { get; }

        public string FacultyTag { get; }

        private string GroupNumber { get; }

        public override int GetHashCode()
        {
            return HashCode.Combine(FacultyTag, Course, GroupNumber);
        }

        public override bool Equals(object obj)
        {
            if (obj is not GroupName item)
            {
                return false;
            }

            return Equals(GroupNumber, item.GroupNumber)
                   && Equals(FacultyTag, item.FacultyTag)
                   && Equals(Course, item.Course);
        }

        public override string ToString()
        {
            return FacultyTag + Course + GroupNumber;
        }

        public GroupNameBuilder ToBuilder()
        {
            GroupNameBuilder groupNameBuilder = new GroupNameBuilder()
                .WithTag(FacultyTag)
                .WithGroupNumber(GroupNumber)
                .WithCourseNumber(Course);

            return groupNameBuilder;
        }

        public class GroupNameBuilder
        {
            private string _facultyTag;
            private string _groupNumber;
            private uint _courseNumber;

            public GroupNameBuilder WithTag(string tag)
            {
                _facultyTag = tag;
                return this;
            }

            public GroupNameBuilder WithGroupNumber(string number)
            {
                _groupNumber = number;
                return this;
            }

            public GroupNameBuilder WithCourseNumber(uint course)
            {
                _courseNumber = course;
                return this;
            }

            public GroupName Build()
            {
                var finallyGroupName = new GroupName(_facultyTag, _courseNumber, _groupNumber);
                return finallyGroupName;
            }
        }
    }
}