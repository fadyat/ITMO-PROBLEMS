using System.Collections.Generic;

namespace Banks.Banks.Limits
{
    public interface ILimit
    {
        public double DebitPercent { get; }

        public SortedDictionary<int, double> DepositPercent { get; }

        public int CreditLimit { get; }

        public double CreditCommission { get; }
    }
}