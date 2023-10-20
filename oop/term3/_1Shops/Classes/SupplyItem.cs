namespace Shops.Classes
{
    public class SupplyItem
    {
        private readonly Product _product;
        private readonly int _price;
        private readonly int _quantity;

        public SupplyItem(Product product, int price, int quantity)
        {
            _product = product;
            _price = price;
            _quantity = quantity;
        }

        public override string ToString()
        {
            return _product + " " + _price + " " + _quantity;
        }
    }
}