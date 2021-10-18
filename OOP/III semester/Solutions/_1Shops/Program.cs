using System;
using System.Collections.Generic;
using Shops.Classes;

namespace Shops
{
    internal static class Program
    {
        private static void Main()
        {
            var a = new ShopManager();
            Shop b = a.CreateShop("Lenta");
            List<Product> c = a.RegisterProducts(new List<string>
            {
                "apple",
                "banana",
            });

            a.AddProducts(b, c, new List<ProductInfo>
            {
                new ProductInfo.ProductInfoBuilder()
                    .WithPrice(100)
                    .WithQuantity(10)
                    .Build(),
                new ProductInfo.ProductInfoBuilder()
                    .WithPrice(200)
                    .WithQuantity(20)
                    .Build(),
            });

            a.Info();
        }
    }
}
