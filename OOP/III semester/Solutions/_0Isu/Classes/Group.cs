namespace Isu.Classes
{
    public class Group
    {
        public Group(GroupName groupName, uint maxCapacity = 30)
        {
            Name = groupName;
            MaxCapacity = maxCapacity;
        }

        public uint MaxCapacity { get; }

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