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
        public double[] CreditLimit { get; protected init; }

        public double CreditCommission { get; protected init; }

        /* All */
        public double WithDrawLimit { get; protected init; }

        public double TransferLimit { get; protected init; }
    }
}