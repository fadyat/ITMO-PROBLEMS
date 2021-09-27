namespace Shops.Classes
{
    public class ProductInfo
    {
        public ProductInfo(uint quantity, double price = -1) { (Quantity, Price) = (quantity, price); }

        public uint Quantity { get; set; }
        public double Price { get; set; }

        public override string ToString() { return Quantity + " " + Price.ToString(".##"); }
    }
}