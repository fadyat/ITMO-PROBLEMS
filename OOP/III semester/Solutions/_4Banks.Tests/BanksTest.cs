using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Factory;
using Banks.Banks.Limits;
using Banks.Clients;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private IBankFactory _bankFactory;
        private Limit _stdLimit;
        private IBank _firstBank;
        private IBank _secondBank;
        private IClient _firstClient;
        private IClient _secondClient;

        [SetUp]
        public void SetUp()
        {
            _bankFactory = new SimpleBankFactory();

            _stdLimit = new SimpleBankLimit(
                3,
                new SortedDictionary<int, double> {{0, 3}, {10000, 4}},
                (-10000, 1e5),
                10,
                1000,
                1000);

            _firstBank = _bankFactory.CreateBank(_stdLimit);
            _secondBank = _bankFactory.CreateBank(_stdLimit);
            CentralBank.AddBank(_firstBank);
            CentralBank.AddBank(_secondBank);

            _firstClient = new Client("first", "client");
            _secondClient = new Client("second", "client");
        }

        [Test]
        public void AddClient()
        {
            _firstBank.AddClient(_firstClient);
            Assert.True(_firstBank.Clients.Contains(_firstClient));
        }

        private static readonly object[] RegisterAccountData =
        {
            new object[]
            {
                new DebitAccount(1000, DateTime.Now)
            },
            new object[]
            {
                new DepositAccount(1000, DateTime.Now, DateTime.Now.AddYears(2))
            },
            new object[]
            {
                new CreditAccount(1000, DateTime.Now)
            },
        };

        [TestCaseSource(nameof(RegisterAccountData))]
        public void RegisterAccount(Account account)
        {
            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);

            Assert.True(_firstBank.Accounts[_firstClient.Id].Contains(account));
        }
    }
}