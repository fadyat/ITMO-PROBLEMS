using NUnit.Framework;
using Shops.Classes;
using Shops.Exceptions;

namespace Shops.Tests
{
    public class ShopManagerTest
    {
        private ShopManager _shopManager;

        [SetUp]
        public void SetUp() { _shopManager = new ShopManager(); }

        /*
            2. Add products to shop.
            3. Purchase products.
            4. Cheapest shops search on a lot shops.
         */
        
        [Test]
        public void RegisterEqualProducts_ThrowException()
        {
            Assert.Catch<ShopException>(() =>
            {
                _shopManager.RegisterProduct("pineapple");
                _shopManager.RegisterProduct("pineapple");
            });
        }
        
        // [Test]
        // public void Che
    }
}