using System.Collections.Generic;
using Shops.Classes;
using Shops.Exceptions;
using Shops.Repositories.Interfaces;
using Shops.Services.Interfaces;

namespace Shops.Services
{
    public class ShopService : IShopService
    {
        private int _issuedShopId;

        public ShopService(
            IShopRepository shopRepository,
            IProductRepository productRepository)
        {
            Shops = shopRepository;
            ProductRepository = productRepository;
            _issuedShopId = 100000;
        }

        public IShopRepository Shops { get; }

        private IProductRepository ProductRepository { get; }

        public Shop CreateShop(string name)
        {
            Shop newShop = new Shop.ShopBuilder()
                .WithName(name)
                .WithId(_issuedShopId++)
                .Build();

            Shops.Save(newShop);
            return newShop;
        }

        public Shop CheapestShopFinding(List<Product> products, List<int> amounts)
        {
            if (products.Count != amounts.Count)
                throw new ProductException("Not enough data!");

            var pricePerListInShop = new Dictionary<int, int?>();
            for (int i = 0; i < products.Count; i++)
            {
                foreach (Product product in ProductRepository.GetAll())
                {
                    if (product.Id != products[i].Id) continue;

                    if (!pricePerListInShop.ContainsKey(product.ShopId))
                    {
                        pricePerListInShop.Add(product.ShopId, 0);
                    }

                    if (product.Quantity < amounts[i])
                    {
                        pricePerListInShop[product.ShopId] = null;
                    }

                    if (pricePerListInShop[product.ShopId] != null)
                    {
                        pricePerListInShop[product.ShopId] += product.Price * amounts[i];
                    }
                }
            }

            int id = 0;
            int minPrice = (int)1e9;
            foreach ((int shopId, int? price) in pricePerListInShop)
            {
                if (price == null || !(price < minPrice)) continue;
                minPrice = (int)price;
                id = shopId;
            }

            return Shops.GetShop(id);
        }
    }
}