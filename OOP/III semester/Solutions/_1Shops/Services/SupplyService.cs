using System.Collections.Generic;
using Shops.Classes;

namespace Shops.Services
{
    public class SupplyService
    {
        private readonly List<Supply> _supplies;
        private readonly int _issuedId;

        public SupplyService()
        {
            _supplies = new List<Supply>();
            _issuedId = 100000;
        }

        private SupplyService(List<Supply> supplies, int id)
        {
            _supplies = supplies;
            _issuedId = id;
        }

        public SupplyServiceBuilder ToBuilder()
        {
            SupplyServiceBuilder supplyServiceBuilder = new SupplyServiceBuilder()
                .WithSupplies(_supplies)
                .WithId(_issuedId);

            return supplyServiceBuilder;
        }

        public class SupplyServiceBuilder
        {
            private List<Supply> _supplies;
            private int _issuedId;

            public SupplyServiceBuilder()
            {
                _supplies = new List<Supply>();
                _issuedId = 100000;
            }

            public SupplyServiceBuilder WithSupplies(List<Supply> supplies)
            {
                _supplies = supplies;
                return this;
            }

            public SupplyServiceBuilder WithId(int id)
            {
                _issuedId = id;
                return this;
            }

            public SupplyServiceBuilder AddSupply(Product product, int price, int quantity)
            {
                var newSupply = new Supply(product, price, quantity);
                _supplies.Add(newSupply);
                return this;
            }

            public SupplyService Build()
            {
                var finallySupplyService = new SupplyService(_supplies, _issuedId);
                return finallySupplyService;
            }
        }
    }
}