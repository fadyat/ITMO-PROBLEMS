using System;
using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        Shop CreateShop(string shopName, string shopAddress);
        Product RegisterProduct(string productName);
        Shop CheapProductSearch(List<(Product, ProductInfo)> productsToBuyCheap);
        void AddProducts(Shop shop, List<(Product, ProductInfo)> productsToSetPrices, List<double> productPrices);
        void PurchaseProduct(Customer customer, Shop shop, List<(Product, ProductInfo)> productsToPurchase);
    }
}