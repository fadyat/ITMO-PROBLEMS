using Banks.Clients;

namespace Banks.Banks.Chain
{
    public class ClientHandler : Handler
    {
        private readonly IClient _client;

        public ClientHandler(IClient client, Handler successor = null)
            : base(successor)
        {
            _client = client;
        }

        public override bool HandlerRequest()
        {
            if (_client.Address == null || _client.Passport == null)
            {
                return false;
            }

            return base.HandlerRequest();
        }
    }
}