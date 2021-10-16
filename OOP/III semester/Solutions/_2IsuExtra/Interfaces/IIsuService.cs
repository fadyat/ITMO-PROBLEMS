using System.Collections.Generic;
using IsuExtra.Classes.Inherited;
using IsuExtra.Classes.New;

namespace IsuExtra.Interfaces
{
    public interface IIsuService : Isu.Interfaces.IIsuService
    {
        Course AddCourse(string facultyTag, Lesson courseInfo, uint maxCapacity = 30);
        Faculty AddFaculty(string facultyTag, IEnumerable<Lesson> schedule);
        void SubscribeCourse(Student student, Course subCourse);
        void UnsubscribeCourse(Student student, Course unsubCourse);
    }
}