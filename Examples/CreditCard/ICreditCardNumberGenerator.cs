using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.CreditCard
{
    public interface ICreditCardNumberGenerator
    {
        /// <summary>
        /// Generates a credit card number
        /// </summary>
        /// <returns>Generated number</returns>
        public string Generate();
    }
}
