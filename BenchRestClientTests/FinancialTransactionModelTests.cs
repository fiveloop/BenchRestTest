using BenchRestClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BenchRestClient.Tests
{
    /// <summary>
    /// Tests for FinancialTransactionModel
    /// </summary>
    [TestClass()]
    public class FinancialTransactionModelTests
    {
        [TestMethod()]
        public void FinancialTransactionModelTest()
        {
            FinancialTransactionModel model = new FinancialTransactionModel();

            Assert.IsNotNull(model.DailyTotals);
            Assert.IsNotNull(model.FinancialTransactions);
        }

        [TestMethod()]
        public void AddFinancialTransactionsTest()
        {
            List<FinancialTransaction> financialTransactions = new List<FinancialTransaction>();
            FinancialTransactionModel model = new FinancialTransactionModel();
            
            FinancialTransaction ft1 = new FinancialTransaction();
            ft1.Date = new DateTime(2013, 12, 22);
            ft1.Ledger = "Phone & Internet Expense";
            ft1.Amount = (decimal)-110.71;
            ft1.Company = "SHAW CABLESYSTEMS CALGARY AB";
            financialTransactions.Add(ft1);

            FinancialTransaction ft2 = new FinancialTransaction();
            ft2.Date = new DateTime(2013, 12, 21);
            ft2.Ledger = "Travel Expense, Nonlocal";
            ft2.Amount = (decimal)-8.1;
            ft2.Company = "BLACK TOP CABS VANCOUVER BC";
            financialTransactions.Add(ft2);

            model.AddFinancialTransactions(financialTransactions);

            Assert.AreEqual(2, model.FinancialTransactions.Count);
            CollectionAssert.Contains(model.FinancialTransactions, ft1);
            CollectionAssert.Contains(model.FinancialTransactions, ft2);
        }

        [TestMethod()]
        public void AddFinancialTransactionTest()
        {
            FinancialTransactionModel model = new FinancialTransactionModel();
            FinancialTransaction ft = new FinancialTransaction();
            ft.Date = new DateTime(2013, 12, 22);
            ft.Ledger = "Phone & Internet Expense";
            ft.Amount = (decimal)-110.71;
            ft.Company = "SHAW CABLESYSTEMS CALGARY AB";

            model.AddFinancialTransaction(ft);

            Assert.AreEqual(1, model.FinancialTransactions.Count);
            CollectionAssert.Contains(model.FinancialTransactions, ft);
        }

        [TestMethod()]
        public void GetRunningDailyBalancesTest()
        {
            List<FinancialTransaction> financialTransactions = new List<FinancialTransaction>();
            FinancialTransactionModel model = new FinancialTransactionModel();

            FinancialTransaction ft1 = new FinancialTransaction();
            ft1.Date = new DateTime(2013, 12, 12);
            ft1.Ledger = "Office Expense";
            ft1.Amount = (decimal)-25.05;
            ft1.Company = "AA OFFICE SUPPLIES";
            financialTransactions.Add(ft1);

            FinancialTransaction ft2 = new FinancialTransaction();
            ft2.Date = new DateTime(2013, 12, 12);
            ft2.Ledger = "Insurance Expense";
            ft2.Amount = (decimal)-20;
            ft2.Company = "AA OFFICE SUPPLIES";
            financialTransactions.Add(ft2);

            FinancialTransaction ft3 = new FinancialTransaction();
            ft3.Date = new DateTime(2013, 12, 13);
            ft3.Ledger = "Business Meals & Entertainment Expense";
            ft3.Amount = (decimal)-10.5;
            ft3.Company = "MCDONALDS RESTAURANT";
            financialTransactions.Add(ft3);

            FinancialTransaction ft4 = new FinancialTransaction();
            ft4.Date = new DateTime(2013, 12, 14);
            ft4.Ledger = "Credit Card - 1234";
            ft4.Amount = (decimal)25;
            ft4.Company = "PAYMENT - THANK YOU";
            financialTransactions.Add(ft4);

            model.AddFinancialTransactions(financialTransactions);
            SortedList<DateTime, decimal> dailyBalances = model.GetRunningDailyBalances();
            CollectionAssert.Contains(dailyBalances, new KeyValuePair<DateTime, decimal>(new DateTime(2013, 12, 12), (decimal)-45.05));
            CollectionAssert.Contains(dailyBalances, new KeyValuePair<DateTime, decimal>(new DateTime(2013, 12, 13), (decimal)-55.55));
            CollectionAssert.Contains(dailyBalances, new KeyValuePair<DateTime, decimal>(new DateTime(2013, 12, 14), (decimal)-30.55));
        }
    }
}
