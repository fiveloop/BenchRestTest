using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BenchRestClient
{    
	/// <summary>
    /// Represents a financial transaction, including the transaction date, ledger, amount and company.
    /// </summary>
    public class FinancialTransaction
    {
		[JsonPropertyName("Date")]
		public DateTime Date { get; set; }
		[JsonPropertyName("Ledger")]
		public string Ledger { get; set; }
		[JsonPropertyName("Amount")]
		//This attribute is to prevent the JsonException "The JSON value could not be converted to System.Decimal"
		[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
		public decimal Amount { get; set; }
		[JsonPropertyName("Company")]
		public string Company { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FinancialTransaction()
        {
            Date = DateTime.MinValue;
            Amount = 0;
            Company = "";
            Ledger = "";
        }

        /// <summary>
        /// Constructor that takes date, ledger, amount and company
        /// </summary>
        /// <param name="date">transaction date</param>
        /// <param name="ledger">transaction ledger</param>
        /// <param name="amount">transaction amount</param>
        /// <param name="company">transaction company</param>
        public FinancialTransaction(DateTime date, string ledger, decimal amount, string company)
        {
            Date = date;
            Ledger = ledger; 
            Amount = amount;
            Company = company;
        }

        /// <summary>
        /// Converts the value of the current object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the value of the current object.</returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}", Date.ToString("yyyy-MM-dd"), Ledger, Amount, Company);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">The object to compare to this instance.</param>
        /// <returns>true if value is an instance of this class and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            FinancialTransaction other = obj as FinancialTransaction;
            return  Date == other.Date &&
                Ledger == other.Ledger &&
                Amount == other.Amount &&
                Company == other.Company;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>the hash code</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Date, Ledger, Amount, Company);
        }
    }
}
