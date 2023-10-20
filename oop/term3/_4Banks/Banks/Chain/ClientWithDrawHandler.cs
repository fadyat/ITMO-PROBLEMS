using Banks.Banks.Limits;
using Banks.Clients;

namespace Banks.Banks.Chain
{
    public class ClientWithDrawHandler : Handler
    {
        private readonly IClient _client;
        private readonly Limit _limit;
        private readonly double _amount;

        public ClientWithDrawHandler(IClient client, Limit limit, double amount, Handler successor = null)
            : base(successor)
        {
            _client = client;
            _limit = limit;
            _amount = amount;
        }

        public override bool HandlerRequest()
        {
            if (Equals(_client.Address, null) &&
                _amount > _limit.WithDrawLimit)
            {
                return false;
            }

            if (Equals(_client.Passport, null) &&
                _amount > _limit.WithDrawLimit)
            {
                return false;
            }

            return base.HandlerRequest();
        }
    }
}