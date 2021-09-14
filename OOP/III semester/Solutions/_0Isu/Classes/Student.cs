using System;

namespace Isu.Classes
{
    public class Student
    {
        private readonly string _name;
        private readonly uint _id;
        public Student(string name,  uint id)
            { (_name, _id) = (name, id); }

        public uint Id { get => _id; }
        public string Name { get => _name; }

        public override string ToString() { return _name + " | " + Convert.ToString(_id); }
    }
}