namespace Isu.Classes
{
    public class Student
    {
        private readonly string _name;
        private readonly uint _id;
        public Student(string studentName,  uint studentId)
            { (_name, _id) = (studentName, studentId); }

        public uint Id => _id;
        public string Name => _name;
        public Group Group { get; set; }

        public override string ToString() { return Group + ": " + _name + " " + _id; }
    }
}