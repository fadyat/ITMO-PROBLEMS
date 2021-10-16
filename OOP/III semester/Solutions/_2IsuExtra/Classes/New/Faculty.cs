using System.Collections.Immutable;
using System.Linq;

namespace IsuExtra.Classes.New
{
    public class Faculty
    {
        public Faculty(string tagFaculty, ImmutableList<Lesson> facultySchedule)
        {
            Tag = tagFaculty;
            Schedule = facultySchedule;
        }

        public string Tag { get; }

        public ImmutableList<Lesson> Schedule { get; }

        public bool CrossingSchedule(Lesson pickedCourse)
        {
            return Schedule.Any(lesson => lesson.CrossingTime(pickedCourse));
        }

        public bool CrossingTeacher(Lesson pickedCourse)
        {
            return Schedule.Any(lesson => lesson.CrossingTime(pickedCourse) &&
                                          lesson.CrossingTeacher(pickedCourse));
        }

        public bool CrossingAuditory(Lesson pickedCourse)
        {
            return Schedule.Any(lesson => lesson.CrossingTime(pickedCourse) &&
                                          lesson.CrossingAuditory(pickedCourse));
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Faculty)obj;
            return other != null && Equals(Tag, other.Tag);
        }

        public override int GetHashCode() { return Tag.GetHashCode(); }
    }
}