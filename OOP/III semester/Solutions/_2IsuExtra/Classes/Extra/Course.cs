namespace IsuExtra.Classes.Extra
{
    public class Course
    {
        public Course(string facultyTag, Lesson info, uint maxCapacity = 30)
        {
            FacultyTag = facultyTag;
            Info = info;
            MaxCapacity = maxCapacity;
        }

        public string FacultyTag { get; }
        public Lesson Info { get; }
        public uint MaxCapacity { get; }
    }
}