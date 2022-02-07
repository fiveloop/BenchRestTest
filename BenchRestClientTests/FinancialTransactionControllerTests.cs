using Microsoft.VisualStudio.TestTools.UnitTesting;
using BenchRestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Moq;
using System.Net;
using System.Text.Json;
using Moq.Protected;
using System.Threading;

namespace BenchRestClient.Tests
{
    /// <summary>
    /// Tests for FinancialTransactionController
    /// </summary>
    [TestClass()]
    public class FinancialTransactionControllerTests
    {
        [TestMethod()]
        public void FinancialTransactionControllerTest()
        {
            HttpClient client = new HttpClient();
            FinancialTransactionController controller = new FinancialTransactionController(client);
            Assert.AreSame(client, controller.Client);
        }

        /*
        [TestMethod()]
        public void DisplayRunningDailyBalancesTest()
        {
            Assert.Fail();
        }
        */

        /*
        [TestMethod()]
        public void GetAllFinancialTransactionPagesTest()
        {
            Assert.Fail();
        }
        */

        /// <summary>
        /// Tests that a 200 OK response and expected JSON content is returned by the web service when it is called 
        /// by GetFinancialTransactionPage.
        /// 
        /// This is a true unit test because I am mocking the HttpClient's HttpMessageHandler so that we don't
        /// call the actual web service.
        /// </summary>
        [TestMethod()]
        public void GetFinancialTransactionPageTestFor200Ok()
        {
            var expectedTransactionPage = new FinancialTransactionPage();
            expectedTransactionPage.TotalCount = 3;
            expectedTransactionPage.Page = 1;
            List<FinancialTransaction> financialTransactions = new List<FinancialTransaction>();

            FinancialTransaction ft1 = new FinancialTransaction();
            ft1.Date = new DateTime(2013, 12, 22);
            ft1.Ledger = "Internet Expense";
            ft1.Amount = (decimal)-110.71;
            ft1.Company = "SHAW CABLESYSTEMS CALGARY AB";
            financialTransactions.Add(ft1);

            FinancialTransaction ft2 = new FinancialTransaction();
            ft2.Date = new DateTime(2013, 12, 21);
            ft2.Ledger = "Travel Expense";
            ft2.Amount = (decimal)-8.1;
            ft2.Company = "BLACK TOP CABS VANCOUVER BC";
            financialTransactions.Add(ft2);

            expectedTransactionPage.FinancialTransactions = financialTransactions;

            var json = JsonSerializer.Serialize<FinancialTransactionPage>(expectedTransactionPage);


            var mockHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json)
            };
            var expectedUri = new Uri("https://resttest.bench.co/transactions/1.json");
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            HttpClient client = new HttpClient(mockHandler.Object);

            FinancialTransactionController controller = new FinancialTransactionController(client);
            FinancialTransactionPage actualTransactionPage = controller.GetFinancialTransactionPage(1);
            //Assert.AreEqual(expectedTransactionPage, actualTransactionPage);
            Assert.AreEqual(expectedTransactionPage.TotalCount, actualTransactionPage.TotalCount);
            Assert.AreEqual(expectedTransactionPage.Page, actualTransactionPage.Page);
            CollectionAssert.AreEquivalent(expectedTransactionPage.FinancialTransactions, actualTransactionPage.FinancialTransactions);
        }

        /// <summary>
        /// Tests that a WebServiceException is thrown when a 404 not found error is returned by the
        /// web service called by GetFinancialTransactionPage.
        /// 
        /// Technically, this is implemented as a integration test, rather than a unit test, because 
        /// the HttpClient is not mocked. This test assumes that a page number of -9999 is not valid
        /// and the web service will return a 404 not found error when we try to retrieve that 
        /// page number.
        /// </summary>
        [TestMethod()]
        public void GetFinancialTransactionPageTestFor404NotFound()
        {
            HttpClient client = new HttpClient();
            FinancialTransactionController controller = new FinancialTransactionController(client);
            WebServiceException actualException = null;
            try
            {
                controller.GetFinancialTransactionPage(-9999);
            }
            catch (WebServiceException ex)
            {
                actualException = ex;
            }

            Assert.IsNotNull(actualException);
            Assert.AreEqual(404, actualException.StatusCode);
        }

        /// <summary>
        /// Tests that an AggegrateException containing an HttpRequestException is thrown when an unknown
        /// host name in used in the web service end point when the web service is called by GetFinancialTransactionPage.
        /// 
        /// Technically, this is implemented as a integration test, rather than a unit test, because 
        /// the HttpClient is not mocked. This test assumes that the host name someunknownhost.resttest.bench.co
        /// does not exist.
        /// </summary>
        [TestMethod()]
        public void GetFinancialTransactionPageTestForUnknownHost()
        {
            HttpClient client = new HttpClient();
            FinancialTransactionController controller = new FinancialTransactionController(client);
            controller.FinancialTransactionsEndpoint = "https://someunknownhost.resttest.bench.co/transactions/{0}.json";
            AggregateException actualException = null;
            try
            {
                controller.GetFinancialTransactionPage(1);
            }
            catch (AggregateException ex)
            {
                actualException = ex;
            }

            Assert.IsNotNull(actualException);
            Assert.IsInstanceOfType(actualException.GetBaseException(), typeof(HttpRequestException));
        }
    }
}