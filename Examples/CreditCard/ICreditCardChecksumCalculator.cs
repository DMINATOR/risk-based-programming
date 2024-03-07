using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.CreditCard
{
    internal interface ICreditCardChecksumCalculator
    {
        /// <summary>
        /// Calculates Checksum for credit card number, using Luhn algorithm
        /// </summary>
        /// <param name="number">Credit card number without checksum</param>
        /// <returns>Single digit</returns>
        public int Calculate(string number);
    }
}
