using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchRestClient
{
    /// <summary>
    /// View implementation for the Bench transactions web service. It displays model data and error
    /// messages. It is the view part of the model-view-controller (MVC) pattern.
    /// 
    /// This view implementation displays output to the Console.
    /// 
    /// Although this application could be implemented without declaring this interface,
    /// I chose to declare this interface to facilitate future testing because interfaces
    /// can be easily mocked or stubbed. Interfaces also allow for alternative implementations
    /// that can be dependency injected into other dependent objects.
    /// </summary>
    public class FinancialTransactionView : IFinancialTransactionView
    {
        /// <summary>
        /// Displays the running daily balances in the view.
        /// 
        /// Each running daily balance is displayed on a new line with the format
        /// yyyy-mm-dd amount. For example: 2013-12-12 -45.05
        /// </summary>
        /// <param name="runningDailyBalances">the running daily balances to display</param>
        public void DisplayRunningDailyBalances(SortedList<DateTime, decimal> runningDailyBalances)
        {
            Trace.WriteLine("Print running daily balances");
            foreach (var rdb in runningDailyBalances)
            {
                Console.WriteLine($"{rdb.Key.ToString("yyyy-MM-dd")} {rdb.Value}");
            }
        }

        /// <summary>
        /// Displays an error message in the view
        /// </summary>
        /// <param name="error">the error message to display</param>
        public void DisplayError(string error)
        {
            Console.WriteLine(error);
        }
    }
}
