using System.Collections.Generic;

namespace Isu.Classes
{
    public class Student
    {
        private readonly HashSet<int> _pickedCourses;
        private readonly HashSet<int> _pickedStreams;

        public Student(Student copyStudent, GroupName newGroupName)
            : this(copyStudent.Name, copyStudent.Id, newGroupName) { }

        public Student(string name, uint id, GroupName groupName)
        {
            Name = name;
            Id = id;
            GroupName = groupName;
            _pickedStreams = new HashSet<int>();
            _pickedCourses = new HashSet<int>();
        }

        public uint Id { get; }

        public string Name { get; }

        public GroupName GroupName { get; }

        // for IsuExtra:
        public IReadOnlyCollection<int> PickedCourses => _pickedCourses;

        public IReadOnlyCollection<int> PickedStreams => _pickedStreams;

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Student)obj;
            return other != null && Id == other.Id;
        }

        public override string ToString()
        {
            return GroupName + ": " + Name + " " + Id;
        }

        public void JoinStream(int id)
        {
            _pickedStreams.Add(id);
        }

        public void LeaveStream(int id)
        {
            _pickedStreams.Remove(id);
        }

        public void JoinCourse(int id)
        {
            _pickedCourses.Add(id);
        }

        public void LeaveCourse(int id)
        {
            _pickedCourses.Remove(id);
        }
    }
}