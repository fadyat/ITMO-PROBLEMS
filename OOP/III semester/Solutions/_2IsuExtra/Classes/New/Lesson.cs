using System;

namespace IsuExtra.Classes.New
{
    public class Lesson
    {
        public Lesson(string name, TimeInterval time, string teacher, uint auditory)
        {
            Name = name;
            DTime = time;
            Teacher = teacher;
            Auditory = auditory;
        }

        public TimeInterval DTime { get; }

        private string Name { get; }

        private string Teacher { get; }

        private uint Auditory { get; }

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
            if (obj is not Lesson item)
            {
                return false;
            }

            return Equals(Name, item.Name) &&
                   Equals(DTime, item.DTime) &&
                   Equals(Teacher, item.Teacher) &&
                   Equals(Auditory, item.Auditory);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DTime);
        }

        public override string ToString()
        {
            return Name + " " + DTime + " " + Teacher + " " + Auditory;
        }
    }
}