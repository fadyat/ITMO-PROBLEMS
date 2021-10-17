using System.Collections.Generic;
using Isu.Classes;
using IsuExtra.Classes.New;

namespace IsuExtra.Interfaces
{
    public interface IIsuService : Isu.Interfaces.IIsuService
    {
        Course AddCourse(string facultyTag, Lesson courseInfo, uint maxCapacity = 30);

        Faculty AddFaculty(string facultyTag, Dictionary<Group, List<Lesson>> flow);

        void AddFacultyFlow(Faculty faculty, Dictionary<Group, List<Lesson>> flows);

        void JoinCourse(Student student, Course subCourse);

        void LeaveCourse(Student student, Course unsubCourse);

        List<Group> GetFaculties(Course course);

        List<Student> StudentsOnCourse(Course course);

        List<Student> UnregisteredStudents(Group group);
    }
}