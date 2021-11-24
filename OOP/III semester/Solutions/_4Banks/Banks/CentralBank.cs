using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Clients;

namespace Banks.Banks
{
    public class CentralBank : ICentralBank
    {
        private readonly List<IBank> _banks;

        public CentralBank()
        {
            _banks = new List<IBank>();
        }

        public void AddBank(IBank bank)
        {
            _banks.Add(bank);
        }

        public void AddClient(IBank bank, IClient client)
        {
            bank = GetBank(bank.Id);
            bank.AddClient(client);
        }

        public IBank GetBank(Guid id)
        {
            return _banks.Single(b => Equals(b.Id, id));
        }

        public void RegisterAccount(IBank bank, IClient client, IAccount account)
        {
            bank = GetBank(bank.Id);
            bank.RegisterAccount(client, account);
        }

        public void Print()
        {
            foreach (IBank b in _banks)
                b.Print();
        }
    }
}