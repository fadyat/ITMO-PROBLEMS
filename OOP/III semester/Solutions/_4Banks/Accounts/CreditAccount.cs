using System;

namespace Banks.Accounts
{
    public class CreditAccount : IAccount
    {
        public void Print()
        {
            Console.Write("\t A: credit");
        }
    }
}