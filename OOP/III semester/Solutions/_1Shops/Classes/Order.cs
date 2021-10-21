namespace Shops.Classes
{
    public class Order
    {
        private Product _product;
        private int _amount;

        public Order(Product product, int amount, int id)
        {
            _product = product;
            _amount = amount;
            Id = id;
        }

        public int Id { get; }
    }
}