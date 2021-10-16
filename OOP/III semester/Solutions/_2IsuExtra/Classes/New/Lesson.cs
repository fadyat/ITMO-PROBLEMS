using System;

namespace IsuExtra.Classes.New
{
    public class Lesson
    {
        public Lesson(string lessonName, TimeInterval time, string teacher, uint auditory)
        {
            Name = lessonName;
            DTime = time;
            Teacher = teacher;
            Auditory = auditory;
        }

        public string Name { get; }
        public TimeInterval DTime { get; }
        public string Teacher { get; }
        public uint Auditory { get; }

        public bool CrossingTime(Lesson other)
        {
            return DTime.CrossingDates(other.DTime) &&
                   DTime.CrossingIntervals(other.DTime);
        }

        public bool CrossingTeacher(Lesson other)
        {
            return CrossingTime(other) &&
                   Equals(Teacher, other.Teacher);
        }

        public bool CrossingAuditory(Lesson other)
        {
            return CrossingTime(other) &&
                   Equals(Auditory, other.Auditory);
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Lesson)obj;
            return other != null && Equals(Name, other.Name) && Equals(DTime, other.DTime)
                   && Equals(Teacher, other.Teacher) && Equals(Auditory, other.Auditory);
        }

        public override int GetHashCode() { return HashCode.Combine(Name, DTime); }
    }
}