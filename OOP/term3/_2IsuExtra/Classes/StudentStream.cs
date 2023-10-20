namespace IsuExtra.Classes
{
    public class StudentStream
    {
        private StudentStream(int id, Lesson lesson, int capacity, int maxCapacity)
        {
            Id = id;
            Info = lesson;
            Capacity = capacity;
            MaxCapacity = maxCapacity;
        }

        public int Id { get; }

        public Lesson Info { get; }

        public int Capacity { get; }

        public int MaxCapacity { get; }

        public override string ToString()
        {
            return Id + " " + Info + " " + MaxCapacity;
        }

        public StudentStreamBuilder ToBuilder()
        {
            StudentStreamBuilder builder = new StudentStreamBuilder()
                .WithId(Id)
                .WithLesson(Info)
                .WithCapacity(Capacity)
                .WithMaxCapacity(MaxCapacity);

            return builder;
        }

        public class StudentStreamBuilder
        {
            private int _id;
            private Lesson _lesson;
            private int _capacity;
            private int _maxCapacity;

            public StudentStreamBuilder WithId(int id)
            {
                _id = id;
                return this;
            }

            public StudentStreamBuilder WithLesson(Lesson lesson)
            {
                _lesson = lesson;
                return this;
            }

            public StudentStreamBuilder WithMaxCapacity(int maxCapacity)
            {
                _maxCapacity = maxCapacity;
                return this;
            }

            public StudentStreamBuilder WithCapacity(int capacity)
            {
                _capacity = capacity;
                return this;
            }

            public StudentStream Build()
            {
                var finallyVersion = new StudentStream(
                    _id,
                    _lesson,
                    _capacity,
                    _maxCapacity);

                return finallyVersion;
            }
        }
    }
}