using System;
using Banks.Exceptions;

namespace Banks.Clients.Passport
{
    public class PassportRu : IPassport
    {
        public PassportRu(string series, string number)
        {
            if (series.Length != 4 || number.Length != 6)
            {
                throw new PassportException("Wrong passport format!");
            }

            Number = string.Concat(series, number);
        }

        public string Number { get; }
    }
}