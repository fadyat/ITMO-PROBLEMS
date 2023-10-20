using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Accounts;
using Banks.Banks;
using Banks.Banks.Limits;
using Banks.Clients;
using NUnit.Framework;

namespace Banks.Tests
{
    public class BanksTest
    {
        private IBank _firstBank;
        private IBank _secondBank;
        private IClient _firstClient;
        private IClient _secondClient;

        [SetUp]
        public void SetUp()
        {
            var stdLimit = new SimpleBankLimit(
                3,
                new SortedDictionary<int, double> {{0, 3}, {10000, 4}},
                 new [] { -1e5, 1e5},
                10,
                150,
                150);

            _firstBank = new Bank("1", Guid.NewGuid(), stdLimit);
            _secondBank = new Bank("2", Guid.NewGuid(), stdLimit);

            _firstClient = new Client("first", "client", Guid.NewGuid());
            _secondClient = new Client("second", "client", Guid.NewGuid());

            CentralBank.AddClient(_firstBank, _firstClient);
            CentralBank.AddClient(_secondBank, _secondClient);
        }

        [Test]
        public void AddClient()
        {
            var client1 = new Client("second", "client", Guid.NewGuid());
            var client2 = new Client("second", "client", Guid.NewGuid(), "???");
            var client3 = new Client("second", "client", Guid.NewGuid(), passport: "???");
            var client4 = new Client("second", "client", Guid.NewGuid(), "???", "???");
            CentralBank.AddClient(_firstBank, client1);
            CentralBank.AddClient(_firstBank, client2);
            CentralBank.AddClient(_firstBank, client3);
            CentralBank.AddClient(_firstBank, client4);
            Assert.True(_firstBank.Clients.Contains(client1));
            Assert.True(_firstBank.Clients.Contains(client2));
            Assert.True(_firstBank.Clients.Contains(client3));
            Assert.True(_firstBank.Clients.Contains(client4));
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
            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
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

            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
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

            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
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

            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
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
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            const double amount = 950;
            double prevBalance = account.Balance;

            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
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

            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
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
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            const double amount = 1e6;
            double prevBalance = account.Balance;

            CentralBank.RegisterAccount(_firstBank, _firstClient, account);
            _firstBank.WithDraw(_firstClient, account, amount);

            Assert.AreEqual(prevBalance, account.Balance);
        }


        private static readonly object[] TransferData =
        {
            new object[]
            {
                new DebitAccount(1e3, DateTime.Now),
                new DebitAccount(1e3, DateTime.Now)
            },
            new object[]
            {
                new DebitAccount(1e3, DateTime.Now),
                new DepositAccount(1e3, DateTime.Now, DateTime.Now.AddMinutes(-1))
            },
            new object[]
            {
                new DebitAccount(1e3, DateTime.Now),
                new CreditAccount(1e3, DateTime.Now)
            },
            new object[]
            {
                new DepositAccount(1e3, DateTime.Now, DateTime.Now.AddMinutes(-1)),
                new DepositAccount(1e3, DateTime.Now, DateTime.Now.AddMinutes(-1))
            },
            new object[]
            {
                new DepositAccount(1e3, DateTime.Now, DateTime.Now.AddMinutes(-1)),
                new CreditAccount(1e3, DateTime.Now)
            },
            new object[]
            {
                new CreditAccount(1e3, DateTime.Now),
                new CreditAccount(1e3, DateTime.Now)
            },
        };

        /*
         * Debit >= 0
         * Deposit - until time end >= 0
         * Credit > down
         * 
         * let's think that deposit account already closed
         * I do Duration = DateTime.Now.AddMinutes(-1)
         * because when I do DateTime.Now they can be different
         */
        [TestCaseSource(nameof(TransferData))]
        public void TransferFromDebit_True_ClientNotEnoughData(Account from, Account to)
        {
            const double amount = 100;

            CentralBank.RegisterAccount(_firstBank, _firstClient, from);
            CentralBank.RegisterAccount(_secondBank, _secondClient, to);

            double prevFromBalance = from.Balance;
            double prevToBalance = to.Balance;
            _firstBank.Transfer(_firstClient, from, to, amount);
            Assert.AreEqual(prevFromBalance - amount, from.Balance);
            Assert.AreEqual(prevToBalance + amount, to.Balance);

            prevFromBalance = from.Balance;
            prevToBalance = to.Balance;
            _secondBank.Transfer(_secondClient, to, from, amount);
            Assert.AreEqual(prevFromBalance + amount, from.Balance);
            Assert.AreEqual(prevToBalance - amount, to.Balance);
        }

        [TestCaseSource(nameof(TransferData))]
        public void TransferFromDebit_True_ClientEnoughData(Account from, Account to)
        {
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            _secondClient = _secondClient.WithAddress("second client address")
                .WithPassport("9876543210");

            const double amount = 2e2;

            CentralBank.RegisterAccount(_firstBank, _firstClient, from);
            CentralBank.RegisterAccount(_secondBank, _secondClient, to);

            double prevFromBalance = from.Balance;
            double prevToBalance = to.Balance;
            _firstBank.Transfer(_firstClient, from, to, amount);
            Assert.AreEqual(prevFromBalance - amount, from.Balance);
            Assert.AreEqual(prevToBalance + amount, to.Balance);

            prevFromBalance = from.Balance;
            prevToBalance = to.Balance;
            _secondBank.Transfer(_secondClient, to, from, amount);
            Assert.AreEqual(prevFromBalance + amount, from.Balance);
            Assert.AreEqual(prevToBalance - amount, to.Balance);
        }

        [TestCaseSource(nameof(TransferData))]
        public void TransferFromDebit_False_ClientNotEnoughData(Account from, Account to)
        {
            const double amount = 200;
            CentralBank.RegisterAccount(_firstBank, _firstClient, from);
            CentralBank.RegisterAccount(_secondBank, _secondClient, to);

            double prevFromBalance = from.Balance;
            double prevToBalance = to.Balance;
            _firstBank.Transfer(_firstClient, from, to, amount);
            Assert.AreEqual(prevFromBalance, from.Balance);
            Assert.AreEqual(prevToBalance, to.Balance);

            prevFromBalance = from.Balance;
            prevToBalance = to.Balance;
            _secondBank.Transfer(_secondClient, to, from, amount);
            Assert.AreEqual(prevFromBalance, from.Balance);
            Assert.AreEqual(prevToBalance, to.Balance);
        }

        [TestCaseSource(nameof(TransferData))]
        public void TransferFromDebit_False_ClientEnoughData(Account from, Account to)
        {
            const double amount = 2e5;
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            _secondClient = _secondClient.WithAddress("second client address")
                .WithPassport("9876543210");

            CentralBank.RegisterAccount(_firstBank, _firstClient, from);
            CentralBank.RegisterAccount(_secondBank, _secondClient, to);

            double prevFromBalance = from.Balance;
            double prevToBalance = to.Balance;
            _firstBank.Transfer(_firstClient, from, to, amount);
            Assert.AreEqual(prevFromBalance, from.Balance);
            Assert.AreEqual(prevToBalance, to.Balance);

            prevFromBalance = from.Balance;
            prevToBalance = to.Balance;
            _secondBank.Transfer(_secondClient, to, from, amount);
            Assert.AreEqual(prevFromBalance, from.Balance);
            Assert.AreEqual(prevToBalance, to.Balance);
        }

        [Test]
        public void CreditCommission()
        {
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            const int days = 15;
            var account1 = new CreditAccount(1000, DateTime.Now);
            CentralBank.RegisterAccount(_firstBank, _firstClient, account1);
            Account account2 = _firstBank.Calculate(_firstClient, account1, DateTime.Now.AddDays(days));
            Assert.AreEqual(account2.Balance, account1.Balance);

            _firstBank.WithDraw(_firstClient, account1, 1100);
            Account account3 = _firstBank.Calculate(_firstClient, account1, DateTime.Now.AddDays(days));
            Assert.AreEqual(account3.Balance, account1.Balance - _firstBank.Limit.CreditCommission * days);
        }

        [Test]
        public void DebitCommission()
        {
            // if we change balance everything will be correct to ofc
            DateTime now = DateTime.Now;
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            var account1 = new DebitAccount(1000, now);
            CentralBank.RegisterAccount(_firstBank, _firstClient, account1);

            DateTime inTime = now.AddMonths(1);
            int days = (inTime - now).Days;
            Account account2 = _firstBank.Calculate(_firstClient, account1, inTime);
            double added = account1.Balance * (_firstBank.Limit.DebitPercent / 100 / 365) * days;
            Assert.AreEqual(
                account2.Balance,
                account1.Balance + added);

            inTime = now.AddDays(15);
            Account account3 = _firstBank.Calculate(_firstClient, account1, inTime);
            Assert.AreEqual(account3.Balance, account1.Balance);

            inTime = now.AddMonths(1).AddDays(15);
            days = (inTime - now).Days;
            Account account4 = _firstBank.Calculate(_firstClient, account1, inTime);
            added = account1.Balance * (_firstBank.Limit.DebitPercent / 100 / 365) * (days - 15);
            Assert.AreEqual(account4.Balance, account1.Balance + added);

            inTime = now.AddMonths(1);
            double days1 = (inTime - now).Days;
            inTime = inTime.AddMonths(1);
            double days2 = (inTime - now.AddMonths(1)).Days;
            Account account5 = _firstBank.Calculate(_firstClient, account1, inTime);
            double added1 = account1.Balance * (_firstBank.Limit.DebitPercent / 100 / 365) * days1;
            double added2 = (account1.Balance + added1) * (_firstBank.Limit.DebitPercent / 100 / 365) * days2;
            Assert.AreEqual(account5.Balance, account1.Balance + added1 + added2);

            inTime = now.AddMonths(1);
            days1 = (inTime - now).Days;
            inTime = inTime.AddMonths(1);
            days2 = (inTime - now.AddMonths(1)).Days;
            inTime = inTime.AddDays(15);
            Account account6 = _firstBank.Calculate(_firstClient, account1, inTime);
            added1 = account1.Balance * (_firstBank.Limit.DebitPercent / 100 / 365) * days1;
            added2 = (account1.Balance + added1) * (_firstBank.Limit.DebitPercent / 100 / 365) * days2;
            Assert.AreEqual(account6.Balance, account1.Balance + added1 + added2);
        }

        [Test]
        public void DepositCommission()
        {
            // deposit commission algo is equal to debit algo
            // but he change percent when balance start be upper
            // ...
            DateTime now = DateTime.Now;
            _firstClient = _firstClient.WithAddress("first client address")
                .WithPassport("1234567890");

            var account1 = new DepositAccount(9999, now, now.AddMonths(3));
            CentralBank.RegisterAccount(_firstBank, _firstClient, account1);

            DateTime inTime = now.AddMonths(1);
            int days1 = (inTime - now).Days;
            inTime = inTime.AddMonths(1);
            int days2 = (inTime - now.AddMonths(1)).Days;
            Account account2 = _firstBank.Calculate(_firstClient, account1, inTime);
            double added1 = account1.Balance * (_firstBank.Limit.DepositPercent[0] / 100 / 365) * days1;
            double added2 = (account1.Balance + added1) * (_firstBank.Limit.DepositPercent[10000] / 100 / 365) * days2;
            Assert.AreEqual(account2.Balance, account1.Balance + added1 + added2);
        }
    }
}