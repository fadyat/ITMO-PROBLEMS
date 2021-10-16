namespace Isu.Classes
{
    public class Student
    {
        public Student(Student copyStudent, Group copyGroup)
            : this(copyStudent.Name, copyStudent.Id, copyGroup) { }

        public Student(string studentName, uint studentId, Group studentGroup)
        {
            Name = studentName;
            Id = studentId;
            Group = studentGroup;
        }

        public uint Id { get; }
        public string Name { get; }
        public Group Group { get; }

        public override int GetHashCode() { return Id.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Student)obj;
            return other != null && Id == other.Id;
        }

        public override string ToString() { return Group + ": " + Name + " " + Id; }
    }
}