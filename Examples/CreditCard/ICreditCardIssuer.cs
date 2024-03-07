using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples.CreditCard
{
    internal interface ICreditCardIssuer
    {
        /// <summary>
        /// Issues new card for a customer
        /// </summary>
        /// <param name="date">Date time to use as a current date</param>
        /// <param name="FirstName">First name</param>
        /// <param name="LastName">Last name</param>
        /// <returns>New card details</returns>
        public CreditCard IssueCard(DateTime date, string FirstName, string LastName);
    }

    internal interface ICreditCardIssuerRisksMitigated
    {
        /// <summary>
        /// Issues new card for a customer
        /// </summary>
        /// <param name="FirstName">First name</param>
        /// <param name="LastName">Last name</param>
        /// <returns>New card details</returns>
        public CreditCard IssueCard(string FirstName, string LastName);
    }
}
