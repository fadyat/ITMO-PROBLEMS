using System.Collections.Generic;
using IsuExtra.Classes;

namespace IsuExtra.Interfaces
{
    public interface IIsuService
    {
        public Student AddStudentToStream(Student student, StudentStream stream);

        public Student DeleteStudentFromStream(Student student, StudentStream stream);

        public Student AddStudentToElectiveCourse(Student student, ElectiveCourse electiveCourse);

        public Student DeleteStudentFromCourse(Student student, ElectiveCourse electiveCourse);

        public List<Student> FindStudents(ElectiveCourse electiveCourse, StudentStream stream);

        public List<Student> FindStudentsNotPickedElectiveCourse(Isu.Classes.GroupName groupName);
    }
}