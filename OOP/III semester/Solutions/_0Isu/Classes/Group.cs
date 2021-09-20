using System.Collections.Generic;

namespace Isu.Classes
{
    public class Group
    {
        private readonly GroupName _groupName;
        private readonly List<Student> _studentsList;

        public Group(GroupName groupName) { (_groupName, _studentsList) = (groupName, new List<Student>()); }

        public GroupName Name => _groupName;
        public List<Student> StudentList => _studentsList;
        public int Capacity { get; set; }
        public int MaxCapacity => 30;

        public static bool operator ==(Group a, Group b) { return Equals(a, b); }

        public static bool operator !=(Group a, Group b) { return !Equals(a, b); }

        public override int GetHashCode() { return _groupName.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Group)obj;
            return other != null && _groupName == other._groupName;
        }

        public override string ToString() { return _groupName.ToString(); }
    }
}