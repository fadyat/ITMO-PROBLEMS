using System.Collections.Generic;
using Shops.Interfaces;

namespace Shops.Classes
{
    public class Customer : ICustomer
    {
        private readonly string _name;
        private readonly Dictionary<Product, ProductInfo> _purchasedProducts;

        public Customer(string customerName, double startMoney)
        {
            (_name, Money) = (customerName, startMoney);
            _purchasedProducts = new Dictionary<Product, ProductInfo>();
        }

        public double Money { get; set; }
        public string Name => _name;
        public Dictionary<Product, ProductInfo> PurchasedProducts => _purchasedProducts;
        public override string ToString() { return _name + " " + Money.ToString(".##"); }
    }
}