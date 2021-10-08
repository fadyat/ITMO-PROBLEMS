namespace Shops.Classes
{
    public class Customer
    {
        public Customer(string customerName, uint startMoney)
        {
            (Name, Money) = (customerName, startMoney);
        }

        public uint Money { get; }
        public string Name { get; }

        public override string ToString()
        {
            return Name + " " + (Money / 100) + "." + (Money % 100);
        }
    }
}