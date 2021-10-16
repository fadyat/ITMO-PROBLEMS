using Isu.Classes;

namespace IsuExtra.Classes.Inherited
{
    public class Student : Isu.Classes.Student
    {
        public Student(string studentName, uint studentId, Group studentGroup)
            : base(studentName, studentId, studentGroup) { }

        public string FacultyTag => Group.Name.FacultyTag;
    }
}