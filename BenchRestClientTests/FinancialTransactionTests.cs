using Microsoft.VisualStudio.TestTools.UnitTesting;
using BenchRestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchRestClient.Tests
{
    /// <summary>
    /// Tests for FinancialTransaction
    /// </summary>
    [TestClass()]
    public class FinancialTransactionTests
    {
        [TestMethod()]
        public void ToStringTest()
        {
            FinancialTransaction ft = new FinancialTransaction();
            ft.Date = new DateTime(2013, 12, 22);
            ft.Ledger = "Phone & Internet Expense";
            ft.Amount = (decimal)-110.71;
            ft.Company = "SHAW CABLESYSTEMS CALGARY AB";

            string expected = String.Format("{0}, {1}, {2}, {3}", ft.Date.ToString("yyyy-MM-dd"), ft.Ledger, ft.Amount, ft.Company);
            Assert.AreEqual(expected, ft.ToString());
        }

        [TestMethod()]
        public void EqualsTestOnEqualObjects()
        {
            FinancialTransaction ft1 = new FinancialTransaction();
            ft1.Date = new DateTime(2013, 12, 22);
            ft1.Ledger = "Phone & Internet Expense";
            ft1.Amount = (decimal)-110.71;
            ft1.Company = "SHAW CABLESYSTEMS CALGARY AB";

            FinancialTransaction ft2 = new FinancialTransaction();
            ft2.Date = new DateTime(2013, 12, 22);
            ft2.Ledger = "Phone & Internet Expense";
            ft2.Amount = (decimal)-110.71;
            ft2.Company = "SHAW CABLESYSTEMS CALGARY AB";

            Assert.IsTrue(ft1.Equals(ft2));
        }

        [TestMethod()]
        public void EqualsTestOnUnequalObjects()
        {
            FinancialTransaction ft1 = new FinancialTransaction();
            ft1.Date = new DateTime(2013, 12, 22);
            ft1.Ledger = "Phone & Internet Expense";
            ft1.Amount = (decimal)-110.71;
            ft1.Company = "SHAW CABLESYSTEMS CALGARY AB";

            FinancialTransaction ft2 = new FinancialTransaction();
            ft2.Date = new DateTime(2014, 11, 19);
            ft2.Ledger = "Office Expense";
            ft2.Amount = (decimal)5.99;
            ft2.Company = "Staples";

            Assert.IsFalse(ft1.Equals(ft2));
        }

        [TestMethod()]
        public void GetHashCodeTestOnEqualObjects()
        {
            FinancialTransaction ft1 = new FinancialTransaction();
            ft1.Date = new DateTime(2013, 12, 22);
            ft1.Ledger = "Phone & Internet Expense";
            ft1.Amount = (decimal)-110.71;
            ft1.Company = "SHAW CABLESYSTEMS CALGARY AB";

            FinancialTransaction ft2 = new FinancialTransaction();
            ft2.Date = new DateTime(2013, 12, 22);
            ft2.Ledger = "Phone & Internet Expense";
            ft2.Amount = (decimal)-110.71;
            ft2.Company = "SHAW CABLESYSTEMS CALGARY AB";

            Assert.AreEqual(ft1.GetHashCode(), ft2.GetHashCode());  
        }
    }
}