using System;
using System.Collections.Generic;

namespace BenchRestClient
{
    /// <summary>
    /// Interface for a view for the Bench transactions web service. It displays model data and error
    /// messages. It is the view part of the model-view-controller (MVC) pattern.
    /// 
    /// Although this application could be implemented without declaring this interface,
    /// I chose to declare this interface to facilitate future testing because interfaces
    /// can be easily mocked or stubbed. Interfaces also allow for alternative implementations
    /// that can be dependency injected into other dependent objects.
    /// </summary>
    public interface IFinancialTransactionView
    {
        /// <summary>
        /// Displays the running daily balances in the view
        /// </summary>
        /// <param name="runningDailyBalances">the running daily balances to display</param>
        void DisplayRunningDailyBalances(SortedList<DateTime, decimal> runningDailyBalances);

        /// <summary>
        /// Displays an error message in the view
        /// </summary>
        /// <param name="error">the error message to display</param>
        void DisplayError(string error);
    }
}