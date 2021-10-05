namespace Shops.Classes
{
    public class Customer
    {
        public Customer(string customerName, double startMoney)
        {
            (Name, Money) = (customerName, startMoney);
        }

        public double Money { get; }
        public string Name { get; }

        public override string ToString() { return Name + " " + Money.ToString(".##"); }
    }
}