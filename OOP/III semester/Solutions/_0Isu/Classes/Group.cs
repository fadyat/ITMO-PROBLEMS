namespace Isu.Classes
{
    public class Group
    {
        public Group(Group copyGroup, uint newCapacity)
            : this(copyGroup.Name, newCapacity, copyGroup.MaxCapacity) { }

        public Group(GroupName groupName, uint capacity, uint maxCapacity = 30)
        {
            Name = groupName;
            Capacity = capacity;
            MaxCapacity = maxCapacity;
        }

        public uint MaxCapacity { get; }

        public uint Capacity { get; }

        public GroupName Name { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is Group item &&
                   Equals(Name, item.Name);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}