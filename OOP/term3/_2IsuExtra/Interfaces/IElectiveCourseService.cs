using System.Collections.Generic;
using IsuExtra.Classes;

namespace IsuExtra.Interfaces
{
    public interface IElectiveCourseService
    {
        ElectiveCourse CreateElectiveCourse(string facultyTag, string name, List<StudentStream> streams);

        ElectiveCourse GetElectiveCourse(int id);

        List<StudentStream> GetStudentStreams(ElectiveCourse electiveCourse);

        void UpdateElectiveCourse(ElectiveCourse electiveCourse);
    }
}