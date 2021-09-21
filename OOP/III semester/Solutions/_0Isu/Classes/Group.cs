namespace Isu.Classes
{
    public class Group
    {
        private readonly GroupName _groupName;
        private readonly uint _maxCapacity;

        public Group(GroupName groupName, uint maxCapacity = 30)
        {
            _groupName = groupName;
            _maxCapacity = maxCapacity;
        }

        public GroupName Name => _groupName;
        public uint Capacity { get; set; }
        public uint MaxCapacity => _maxCapacity;

        public static bool operator ==(Group a, Group b) { return Equals(a, b); }

        public static bool operator !=(Group a, Group b) { return !Equals(a, b); }

        public override int GetHashCode() { return _groupName.GetHashCode(); }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() != GetType()) return false;
            var other = (Group)obj;
            return other != null && _groupName == other._groupName;
        }

        public override string ToString() { return _groupName.ToString(); }
    }
}