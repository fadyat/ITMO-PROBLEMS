namespace Shops.Classes
{
    public class Customer
    {
        private Customer() { }
        private Customer(string customerName, uint startMoney)
        {
            Name = customerName;
            Money = startMoney;
        }

        public uint Money { get; }
        private string Name { get; }

        public override string ToString()
        {
            return Name + " " + (Money / 100) + "." + (Money % 100);
        }

        public CustomerBuilder ToBuilder()
        {
            CustomerBuilder customerBuilder = new ();
            customerBuilder.WithName(Name);
            customerBuilder.WithMoney(Money);
            return customerBuilder;
        }

        public class CustomerBuilder
        {
            private string _name;
            private uint _money;

            public CustomerBuilder WithName(string customerName)
            {
                _name = customerName;
                return this;
            }

            public CustomerBuilder WithMoney(uint customerMoney)
            {
                _money = customerMoney;
                return this;
            }

            public CustomerBuilder PurchaseProduct(ref Shop shop, Product product, uint quantity)
            {
                ProductInfo previousInfo = shop.StoredProducts.GetProductInfo(product);
                shop = shop.ToBuilder()
                    .PurchaseProduct(product, quantity)
                    .Build();

                uint spendingMoney = (previousInfo.Quantity - quantity) * previousInfo.Price;
                _money -= spendingMoney;
                return this;
            }

            public Customer Build()
            {
                Customer finalCustomer = new (_name, _money);
                return finalCustomer;
            }
        }
    }
}