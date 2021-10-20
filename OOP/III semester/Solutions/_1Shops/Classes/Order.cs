namespace Shops.Classes
{
    public class Order
    {
        private Product _product;
        private int _amount;
        private int _id;

        public Order(Product product, int amount, int id)
        {
            _product = product;
            _amount = amount;
            _id = id;
        }
    }
}