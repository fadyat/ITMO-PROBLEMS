using System;
using Banks.Exceptions;

namespace Banks.Clients.Passport
{
    public class PassportRu : IPassport
    {
        public PassportRu(int series, int number)
        {
            if (series.ToString().Length != 4 || number.ToString().Length != 6)
            {
                throw new PassportException("Wrong passport format!");
            }

            Number = Convert.ToInt32(string.Concat(series, number));
        }

        public int Number { get; }
    }
}