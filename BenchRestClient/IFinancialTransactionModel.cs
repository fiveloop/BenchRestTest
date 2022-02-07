using System;
using System.Collections.Generic;

namespace BenchRestClient
{
    /// <summary>
    /// Interface for a model for the Bench transactions web service. It holds the transactions returned
    /// by the web service and does calculations and transformations on those transactions.
    /// It is the model part of the model-view-controller (MVC) pattern.
    /// 
    /// Although this application could be implemented without declaring this interface,
    /// I chose to declare this interface to facilitate future testing because interfaces
    /// can be easily mocked or stubbed. Interfaces also allow for alternative implementations
    /// that can be dependency injected into other dependent objects.
    /// </summary>
    public interface IFinancialTransactionModel
    {
        /// <summary>
        /// Holds transaction totals for each day.
        /// </summary>
        SortedList<DateTime, decimal> DailyTotals { get; }
        /// <summary>
        /// Holds all transactions.
        /// </summary>
        List<FinancialTransaction> FinancialTransactions { get; }
        /// <summary>
        /// Adds a list of transactions to FinancialsTransactions.
        /// </summary>
        /// <param name="financialTransactions">the list of transactions to be added</param>
        void AddFinancialTransactions(List<FinancialTransaction> financialTransactions);
        /// <summary>
        /// Adds a transaction to FinancialTransactions and updates the DailyTotals for the transaction's date.
        /// </summary>
        /// <param name="ft">the transaction to add</param>
        void AddFinancialTransaction(FinancialTransaction ft);
        /// <summary>
        /// Calculates and returns the running balances for each day.
        /// </summary>
        /// <returns>a list of running balances for each day, sorted by the day, ascending.</returns>
        SortedList<DateTime, decimal> GetRunningDailyBalances();
    }
}