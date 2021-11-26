using System;
using System.Collections.Generic;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Factory;
using Banks.Banks.Limits;
using Banks.Clients;
using Banks.Clients.Passport;
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
                (-1e5, 1e5),
                10,
                150,
                150);

            _firstBank = _bankFactory.CreateBank(_stdLimit);
            _secondBank = _bankFactory.CreateBank(_stdLimit);
            CentralBank.AddBank(_firstBank);
            CentralBank.AddBank(_secondBank);

            _firstClient = new Client.ClientBuilder()
                .WithSurname("first")
                .WithName("client")
                .WithId(Guid.NewGuid())
                .Build();

            _secondClient = new Client.ClientBuilder()
                .WithSurname("second")
                .WithName("client")
                .WithId(Guid.NewGuid())
                .Build();
        }

        [Test]
        public void AddClient()
        {
            _firstBank.AddClient(_firstClient);
            Assert.True(_firstBank.Clients.Contains(_firstClient));
        }

        private static readonly object[] DebitAccountData =
        {
            new object[]
            {
                new DebitAccount(1e4, DateTime.Now)
            }
        };

        private static readonly object[] DepositAccountData =
        {
            new object[]
            {
                new DepositAccount(1e4, DateTime.Now, DateTime.Now.AddYears(2))
            }
        };

        private static readonly object[] CreditAccountData =
        {
            new object[]
            {
                new CreditAccount(1e4, DateTime.Now)
            }
        };

        [TestCaseSource(nameof(DebitAccountData))]
        [TestCaseSource(nameof(DepositAccountData))]
        [TestCaseSource(nameof(CreditAccountData))]
        public void RegisterAccount(Account account)
        {
            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);

            Assert.True(_firstBank.Accounts[_firstClient.Id].Contains(account));
        }

        /*
         * CreditAccount <= up
         */
        [TestCaseSource(nameof(DebitAccountData))]
        [TestCaseSource(nameof(DepositAccountData))]
        [TestCaseSource(nameof(CreditAccountData))]
        public void TopUpAccount_True(Account account)
        {
            const double amount = 1e3;
            double prevBalance = account.Balance;

            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);
            _firstBank.TopUp(_firstClient, account, amount);

            Assert.AreEqual(prevBalance + amount, account.Balance);
        }

        /*
         * CreditAccount.Balance > up
         */
        [TestCaseSource(nameof(CreditAccountData))]
        public void TopUpAccount_False(Account account)
        {
            const double amount = 1e5;
            double prevBalance = account.Balance;

            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);
            _firstBank.TopUp(_firstClient, account, amount);

            Assert.AreEqual(prevBalance, account.Balance);
        }

        /*
         * Debit.Balance >= 0
         * Credit.Balance >= down
         * Client(bad): amount <= limit.WithDrawLimit
         */
        [TestCaseSource(nameof(DebitAccountData))]
        [TestCaseSource(nameof(CreditAccountData))]
        public void WithDrawAccount_True_ClientNotEnoughData(Account account)
        {
            const double amount = 50;
            double prevBalance = account.Balance;

            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);
            _firstBank.WithDraw(_firstClient, account, amount);

            Assert.AreEqual(prevBalance - amount, account.Balance);
        }

        /*
         * Debit.Balance >= 0
         * Credit.Balance >= down
         * Client(good): amount > limit.WithDrawLimit
         */
        [TestCaseSource(nameof(DebitAccountData))]
        [TestCaseSource(nameof(CreditAccountData))]
        public void WithDrawAccount_True_ClientEnoughData(Account account)
        {
            _firstClient = _firstClient.ToBuilder()
                .WithAddress("first client address")
                .WithPassport(new PassportRu(1234, 567890))
                .Build();

            const double amount = 950;
            double prevBalance = account.Balance;

            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);
            _firstBank.WithDraw(_firstClient, account, amount);

            Assert.AreEqual(prevBalance - amount, account.Balance);
        }
        
        /*
         * Client(bad): amount > limit.WithDrawLimit
         */
        [TestCaseSource(nameof(DebitAccountData))]
        [TestCaseSource(nameof(DepositAccountData))]
        [TestCaseSource(nameof(CreditAccountData))]
        public void WithDrawAccount_False_ClientNotEnoughData(Account account)
        {
            const double amount = 2e2;
            double prevBalance = account.Balance;

            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);
            _firstBank.WithDraw(_firstClient, account, amount);

            Assert.AreEqual(prevBalance, account.Balance);
        }
        
        /*
         * Debit.Balance < 0
         * Deposit can't do WithDraw()
         * Credit.Balance < down
         * Client(good)
         */
        [TestCaseSource(nameof(DebitAccountData))]
        [TestCaseSource(nameof(DepositAccountData))]
        [TestCaseSource(nameof(CreditAccountData))]
        public void WithDrawAccount_False_ClientEnoughData(Account account)
        {
            _firstClient = _firstClient.ToBuilder()
                .WithAddress("first client address")
                .WithPassport(new PassportRu(1234, 567890))
                .Build();

            const double amount = 1e6;
            double prevBalance = account.Balance;

            _firstBank.AddClient(_firstClient);
            _firstBank.RegisterAccount(_firstClient, account);
            _firstBank.WithDraw(_firstClient, account, amount);

            Assert.AreEqual(prevBalance, account.Balance);
        }
    }
}