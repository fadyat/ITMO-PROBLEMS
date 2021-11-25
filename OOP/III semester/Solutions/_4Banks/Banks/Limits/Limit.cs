using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public abstract class Limit
    {
        /* Debit */
        public double DebitPercent { get; protected init; }

        /* Deposit */
        public SortedDictionary<int, double> DepositPercent { get; protected init; }

        /* Credit */
        public (int down, int up) CreditLimit { get; protected init; }

        public int CreditCommission { get; protected init; }

        public int DaysForRepayment { get; protected init; }
    }
}