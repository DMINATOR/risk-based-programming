using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.CreditCard
{
    public interface ICreditCardCVCGenerator
    {
        /// <summary>
        /// Generates CVC from credit card number
        /// </summary>
        public string Generate(string number);
    }
}
