using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchRestClient
{
    /// <summary>
    /// Model implementation for the Bench transactions web service. It holds the transactions returned
    /// by the web service and does calculations and transformations on those transactions.
    /// It is the model part of the model-view-controller (MVC) pattern.
    /// </summary>
    public class FinancialTransactionModel : IFinancialTransactionModel
    {
        /// <summary>
        /// Holds transaction totals for each day.
        /// </summary>
        public SortedList<DateTime, decimal> DailyTotals { get;}
        /// <summary>
        /// Holds all transactions.
        /// </summary>
        public List<FinancialTransaction> FinancialTransactions { get;}

        /// <summary>
        /// Default constructor. Initializes DailyTotals and FinancialsTransactions.
        /// </summary>
        public FinancialTransactionModel()
        {
            DailyTotals = new SortedList<DateTime, decimal>();
            FinancialTransactions = new List<FinancialTransaction>();
        }

        /// <summary>
        /// Adds a list of transactions to FinancialsTransactions.
        /// </summary>
        /// <param name="financialTransactions">the list of transactions to be added</param>
        public void AddFinancialTransactions(List<FinancialTransaction> financialTransactions)
        {
            foreach (var ft in financialTransactions)
            {
                AddFinancialTransaction(ft);
            }
        }

        /// <summary>
        /// Adds a transaction to FinancialTransactions and updates the DailyTotals for the transaction's date.
        /// </summary>
        /// <param name="ft">the transaction to add</param>
        public void AddFinancialTransaction(FinancialTransaction ft)
        {
            Trace.WriteLine("Adding financial transaction: " + ft.ToString());
            FinancialTransactions.Add(ft);
            Trace.WriteLine("Adding transaction amount to daily totals");
            if (DailyTotals.ContainsKey(ft.Date))
            {
                Trace.WriteLine($"Current amount for {ft.Date.ToString("yyyy-MM-dd")}: {DailyTotals[ft.Date]}");
                DailyTotals[ft.Date] += ft.Amount;
                Trace.WriteLine($"New amount: {DailyTotals[ft.Date]}");
            }
            else
            {
                Trace.WriteLine($"Adding new daily total for {ft.Date.ToString("yyyy-MM-dd")}: {ft.Amount}");
                DailyTotals.Add(ft.Date, ft.Amount);
            }
            Trace.WriteLine("");
        }

        /// <summary>
        /// Calculates and returns the running balances for each day.
        /// </summary>
        /// <returns>a list of running balances for each day, sorted by the day, ascending.</returns>
        public SortedList<DateTime, decimal> GetRunningDailyBalances()
        {
            SortedList<DateTime, decimal> runningDailyBalances = new SortedList<DateTime, decimal>();
            decimal runningBalance = 0;
            foreach (var dt in DailyTotals)
            {
                runningBalance += dt.Value;
                runningDailyBalances.Add(dt.Key, runningBalance);
            }
            return runningDailyBalances;
        }
    }
}
