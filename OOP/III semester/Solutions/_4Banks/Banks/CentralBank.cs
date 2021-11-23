using System.Collections.Generic;
using Banks.Accounts;

namespace Banks.Banks
{
    public class CentralBank
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

        public void BankOperation(IBank bank)
        {
            // calculate percents
        }

        public void AccountOperation(IAccount account)
        {
            // change account status
        }

        public void BanksNotification()
        {
            // notification
        }
    }
}