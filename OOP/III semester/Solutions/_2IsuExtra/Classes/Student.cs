using System.Collections.Generic;

namespace IsuExtra.Classes
{
    public class Student : Isu.Classes.Student
    {
        private readonly HashSet<int> _pickedCourses;
        private readonly HashSet<int> _pickedStreams;

        public Student(string name, uint id, Isu.Classes.GroupName groupName)
            : base(name, id, groupName)
        {
            _pickedCourses = new HashSet<int>();
            _pickedStreams = new HashSet<int>();
        }

        public IReadOnlyCollection<int> PickedCourses => _pickedCourses;

        public IReadOnlyCollection<int> PickedStreams => _pickedStreams;

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