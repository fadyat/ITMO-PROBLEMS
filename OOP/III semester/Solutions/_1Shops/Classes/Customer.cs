namespace Shops.Classes
{
    public class Customer
    {
        public Customer(string customerName, double startMoney)
        {
            (Name, Money) = (customerName, startMoney);
        }

        public double Money { get; private set; }
        public string Name { get; }

        public void SetMoney(double moneyl)
        {
            Money = moneyl;
        }

        public override string ToString() { return Name + " " + Money.ToString(".##"); }
    }
}