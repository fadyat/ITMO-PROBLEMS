namespace Shops.Classes
{
    public class ProductInfo
    {
        public ProductInfo(uint cnt, uint price)
        {
            (Cnt, Price) = (cnt, price);
        }

        public uint Cnt { get; }
        public uint Price { get; }

        public override string ToString()
        {
            return Cnt + " " + (Price / 100) + "." + (Price % 100);
        }
    }
}