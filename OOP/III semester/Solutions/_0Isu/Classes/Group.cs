namespace Isu.Classes
{
    public class Group
    {
        public Group(GroupName groupName, uint maxCapacity = 30)
        {
            Name = groupName;
            MaxCapacity = maxCapacity;
        }

        public GroupName Name { get; }
        public uint MaxCapacity { get; }

        public override int GetHashCode() { return Name.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Group)obj;
            return other != null && Equals(Name, other.Name);
        }

        public override string ToString() { return Name.ToString(); }
    }
}