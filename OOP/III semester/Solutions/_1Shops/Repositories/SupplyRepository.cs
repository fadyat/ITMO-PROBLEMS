using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Classes;
using Shops.Repositories.Interfaces;

namespace Shops.Repositories
{
    public class SupplyRepository : ISupplyRepository
    {
        private readonly List<Supply> _supplies;

        public SupplyRepository()
        {
            _supplies = new List<Supply>();
        }

        public void Save(Supply supply)
        {
            _supplies.Add(supply);
        }

        public IEnumerable<Supply> GetAll()
        {
            return _supplies;
        }

        public Supply GetSupply(int id)
        {
            foreach (Supply supply in _supplies
                .Where(supply => Equals(supply.Id, id)))
            {
                return supply;
            }

            throw new Exception(); // ...
        }
    }
}