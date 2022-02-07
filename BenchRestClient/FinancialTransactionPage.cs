using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BenchRestClient
{
	/// <summary>
	/// Represents a financial transaction page returned by the Bench transactions web service.
	/// It holds a list of the transactions on the page and other page information such as the 
	/// page number and the total number of transactions across all pages.
	/// </summary>
	public class FinancialTransactionPage
    {
		[JsonPropertyName("totalCount")]
		public int TotalCount { get; set; }
		[JsonPropertyName("page")]
		public int Page { get; set; }
		[JsonPropertyName("transactions")]
		public List<FinancialTransaction> FinancialTransactions { get; set; }
	}
}
