namespace Shops.Classes
{
    public class OrderItem
    {
        private readonly Product _product;
        private readonly int _amount;

        public OrderItem(Product product, int amount)
        {
            _product = product;
            _amount = amount;
        }

        public override string ToString()
        {
            return _product + " " + _amount;
        }
    }
}