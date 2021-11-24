using System;

namespace Banks.Accounts
{
    public class DebitAccount : IAccount
    {
        public void Print()
        {
            Console.Write("\t A: debit");
        }
    }
}