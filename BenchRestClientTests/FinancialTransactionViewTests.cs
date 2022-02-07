using Microsoft.VisualStudio.TestTools.UnitTesting;
using BenchRestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BenchRestClient.Tests
{
    /// <summary>
    /// Tests for FinancialTransactionView. The methods of FinancialTransactionView output to the console.
    /// To make test assertions on that output, it's necessary to capture those console outputs. To do that
    /// I used the technique described on this page:
    /// <see href="https://www.aligrant.com/web/blog/2020-07-20_capturing_console_outputs_with_microsoft_test_framework">Capturing Console Outputs With Microsoft Test Framework</see>
    /// </summary>
    [TestClass()]
    public class FinancialTransactionViewTests
    {
        private StringBuilder ConsoleOutput { get; set; }

        [TestInitialize]
        public void Init()
        {
            ConsoleOutput = new StringBuilder();
            Console.SetOut(new StringWriter(this.ConsoleOutput));    // Associate StringBuilder with StdOut
            this.ConsoleOutput.Clear();    // Clear text from any previous text runs
        }

        [TestMethod()]
        public void DisplayRunningDailyBalancesTest()
        {
            SortedList<DateTime, decimal> runningDailyBalances = new SortedList<DateTime, decimal>();
            runningDailyBalances.Add(new DateTime(2013, 12, 12), (decimal)-45.05);
            runningDailyBalances.Add(new DateTime(2013, 12, 13), (decimal)-55.55);
            runningDailyBalances.Add(new DateTime(2013, 12, 14), (decimal)-30.55);

            FinancialTransactionView view = new FinancialTransactionView();
            view.DisplayRunningDailyBalances(runningDailyBalances);
            Assert.IsTrue(this.ConsoleOutput.ToString().Contains("2013-12-12 -45.05"));
            Assert.IsTrue(this.ConsoleOutput.ToString().Contains("2013-12-13 -55.55"));
            Assert.IsTrue(this.ConsoleOutput.ToString().Contains("2013-12-14 -30.55"));
        }

        [TestMethod()]
        public void DisplayErrorTest()
        {
            FinancialTransactionView view = new FinancialTransactionView();
            string error = "some error";
            view.DisplayError(error);
            Assert.IsTrue(this.ConsoleOutput.ToString().Contains(error));
        }
    }
}