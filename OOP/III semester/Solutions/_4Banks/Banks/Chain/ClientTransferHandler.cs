using System;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.UI;
using Spectre.Console;

namespace Banks.Banks.Chain
{
    public class ClientTransferHandler : Handler
    {
        private readonly IClient _client;
        private readonly Limit _limit;
        private readonly double _amount;

        public ClientTransferHandler(IClient client, Limit limit, double amount, Handler successor = null)
            : base(successor)
        {
            _client = client;
            _limit = limit;
            _amount = amount;
        }

        public override bool HandlerRequest()
        {
            if (Equals(_client.Address, null) &&
                _amount > _limit.TransferLimit)
            {
                AnsiConsole.WriteLine("[red]Client don't have address and cross TransferLimit![/]");
                return false;
            }

            if (Equals(_client.Passport, null) &&
                _amount > _limit.TransferLimit)
            {
                AnsiConsole.WriteLine("[red]Client don't have passport and cross TransferLimit![/]");
                return false;
            }

            return base.HandlerRequest();
        }
    }
}