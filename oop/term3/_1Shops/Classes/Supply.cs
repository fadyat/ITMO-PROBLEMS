using System.Collections.Generic;

namespace Shops.Classes
{
    public class Supply
    {
        public Supply(List<SupplyItem> newSupplies, int id)
        {
            Supplies = newSupplies;
            Id = id;
        }

        public List<SupplyItem> Supplies { get; }

        public int Id { get; }
    }
}