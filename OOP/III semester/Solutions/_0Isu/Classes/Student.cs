namespace Isu.Classes
{
    public class Student
    {
        public Student(Student copyStudent, GroupName newGroupName)
            : this(copyStudent.Name, copyStudent.Id, newGroupName) { }

        public Student(string name, uint id, GroupName groupName)
        {
            Name = name;
            Id = id;
            GroupName = groupName;
        }

        public uint Id { get; }

        public string Name { get; }

        public GroupName GroupName { get; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Student)obj;
            return other != null && Id == other.Id;
        }

        public override string ToString()
        {
            return GroupName + ": " + Name + " " + Id;
        }
    }
}