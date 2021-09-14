using System.Collections.Generic;

namespace Isu.Services
{
    public class Group
    {
        private readonly GroupName _groupName;
        private readonly List<Student> _studentsList;
        public Group(GroupName name) { (_groupName, _studentsList) = (name, new List<Student>()); }
        public GroupName Name { get => _groupName; }
        public List<Student> StudentList { get => _studentsList;  }
        public int Capacity { get; set; } = 0;
        public int MaxCapacity { get; } = 30;

        public static bool operator ==(Group a, Group b) { return Equals(a, b); }

        public static bool operator !=(Group a, Group b) { return !Equals(a, b); }

        public override int GetHashCode() { return _groupName.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Group)obj;
            return other != null && _groupName == other._groupName;
        }
    }
}