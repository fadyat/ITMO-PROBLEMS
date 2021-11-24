using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public abstract class ILimit
    {
        public double DebitPercent { get; protected init; }

        public SortedDictionary<int, double> DepositPercent { get; protected init; }

        public (int, int) CreditLimit { get; protected init; }

        public int CreditCommission { get; protected init; }

        public int DaysForRepayment { get; protected init; }
    }
}