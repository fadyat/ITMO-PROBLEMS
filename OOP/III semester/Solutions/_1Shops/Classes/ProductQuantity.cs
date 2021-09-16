using Shops.Interfaces;

namespace Shops.Classes
{
    public class ProductQuantity : IProductQuantity
    {
        public ProductQuantity(uint quantity) { Quantity = quantity; }
        public uint Quantity { get; set; }
    }
}