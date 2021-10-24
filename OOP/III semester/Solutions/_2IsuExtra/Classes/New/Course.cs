namespace IsuExtra.Classes.New
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

        public override bool Equals(object obj)
        {
            if (obj is not Course item)
            {
                return false;
            }

            return Equals(FacultyTag, item.FacultyTag) &&
                   Equals(Info, item.Info);
        }

        public override int GetHashCode()
        {
            return FacultyTag.GetHashCode();
        }

        public override string ToString()
        {
            return FacultyTag + " " + Info + " " + MaxCapacity;
        }
    }
}