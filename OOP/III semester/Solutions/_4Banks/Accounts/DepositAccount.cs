using System;

namespace Banks.Accounts
{
    public class DepositAccount : IAccount
    {
        public void Print()
        {
            Console.Write("\t A: credit");
        }
    }
}