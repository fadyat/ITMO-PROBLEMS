namespace Shops.Classes
{
    public class Customer
    {
        private readonly string _name;

        public Customer(string customerName, double startMoney)
        {
            (_name, Money) = (customerName, startMoney);
        }

        public double Money { get; set; }

        public override string ToString() { return _name + " " + Money.ToString(".##"); }
    }
}