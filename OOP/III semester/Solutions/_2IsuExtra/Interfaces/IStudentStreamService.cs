using IsuExtra.Classes;

namespace IsuExtra.Interfaces
{
    public interface IStudentStreamService
    {
        StudentStream CreateStream(Lesson lesson, int maxCapacity);

        StudentStream GetStream(int id);

        void UpdateStream(StudentStream newStream);
    }
}