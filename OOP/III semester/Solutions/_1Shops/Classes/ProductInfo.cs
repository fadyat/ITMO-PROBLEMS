namespace Shops.Classes
{
    public class ProductInfo
    {
        public ProductInfo(uint cnt, double price = -1) { (Cnt, Price) = (cnt, price); }

        public uint Cnt { get; set; }
        public double Price { get; set; }

        public override string ToString() { return Cnt + " " + Price.ToString(".##"); }
    }
}