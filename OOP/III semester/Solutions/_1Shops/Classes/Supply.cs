namespace Shops.Classes
{
    public class Supply
    {
        private Product _product;
        private int _price;
        private int _quantity;

        public Supply(Product product, int price, int quantity, int id)
        {
            _product = product;
            _price = price;
            _quantity = quantity;
            Id = id;
        }

        public int Id { get; }
    }
}