namespace IsuExtra.Classes.Extra
{
    public class Course
    {
        public Course(string tagFaculty, Lesson info, uint maxCapacity = 30)
        {
            TagFaculty = tagFaculty;
            Info = info;
            MaxCapacity = maxCapacity;
        }

        public string TagFaculty { get; }
        public Lesson Info { get; }
        public uint MaxCapacity { get; }
    }
}