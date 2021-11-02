using System;

namespace IsuExtra.Classes
{
    public class Lesson
    {
        private Lesson(string name, TimeInterval time, string teacher, uint auditory)
        {
            Name = name;
            DTime = time;
            Teacher = teacher;
            Auditory = auditory;
        }

        private TimeInterval DTime { get; }

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
            return Equals(Teacher, other.Teacher);
        }

        public bool CrossingAuditory(Lesson other)
        {
            return Equals(Auditory, other.Auditory);
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

        public LessonBuilder ToBuilder()
        {
            LessonBuilder builder = new LessonBuilder()
                .WithName(Name)
                .WithAuditory(Auditory)
                .WithTeacher(Teacher)
                .WithTime(DTime);

            return builder;
        }

        public class LessonBuilder
        {
            private TimeInterval _time;
            private string _name;
            private uint _auditory;
            private string _teacher;

            public LessonBuilder WithTime(TimeInterval interval)
            {
                _time = interval;
                return this;
            }

            public LessonBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public LessonBuilder WithAuditory(uint auditory)
            {
                _auditory = auditory;
                return this;
            }

            public LessonBuilder WithTeacher(string teacher)
            {
                _teacher = teacher;
                return this;
            }

            public Lesson Build()
            {
                var finallyLesson = new Lesson(_name, _time, _teacher, _auditory);
                return finallyLesson;
            }
        }
    }
}