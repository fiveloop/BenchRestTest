using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BenchRestClient
{
    /// <summary>
    /// Controller implementation for the Bench transactions web service. It calls the web service,
    /// saves the results in the model and updates the view. It is the controller part of the
    /// model-view-controller (MVC) pattern.
    /// </summary>
    public class FinancialTransactionController : IFinancialTransactionController
    {
        /// <summary>
        /// The default URL for the endpoint for the Bench transactions web service.
        /// This application will first try to read the endpoint from the application config file
        /// but if it's not found there, then this default is used.
        /// </summary>
        public const string DefaultFinancialTransactionsEndpoint = "https://resttest.bench.co/transactions/{0}.json";
        /// <summary>
        /// The HttpClient that will be used to call the the Bench transactions web service.
        /// </summary>
        private HttpClient _client;
        public HttpClient Client
        {
            get
            {
                return _client;
            }
            protected set
            { 
                _client = value;
            }
        }
        /// <summary>
        /// The endpoint for the Bench transactions web service.
        /// </summary>
        public string FinancialTransactionsEndpoint { get; set; }
        /// <summary>
        /// The model portion of this model-view-controller triad.
        /// </summary>
        public IFinancialTransactionModel Model { get; set; }
        /// <summary>
        /// The view portion of this model-view-controller triad.
        /// </summary>
        public IFinancialTransactionView View { get; set; }

        /// <summary>
        /// Constructor that takes in the HttpClient
        /// </summary>
        /// <param name="client">the HTTPClient</param>
        public FinancialTransactionController(HttpClient client)
        {
            Client = client;
            FinancialTransactionsEndpoint = DefaultFinancialTransactionsEndpoint;
        }

        /// <summary>
        /// Gets all financial transaction pages from the web service, puts them in the model, and displays the
        /// running daily balances in the view. Resulting exceptions are displayed in the view.
        /// </summary>        
        public void DisplayRunningDailyBalances()
        {
            try
            {
                GetAllFinancialTransactionPages();
                View.DisplayRunningDailyBalances(Model.GetRunningDailyBalances());
            }
            catch (WebServiceException wsEx)
            {
                View.DisplayError($"{wsEx.GetType().Name} error occurred when calling the web service: Status = {wsEx.StatusCode}, Content = {wsEx.Content}");
            }
            catch (AggregateException aggEx)
            {
                aggEx.Handle(inner =>
                {
                    View.DisplayError($"{inner.GetType().Name} error occurred when calling the web service: {inner.Message}");
                    return true;
                });
            }
        }

        /// <summary>
        /// Calls the web service one or more times to retrieve all financial transaction pages.
        /// </summary>
        /// <exception cref="WebServiceException">Thrown when the web service returns a non-200 HTTP status code</exception>
        /// <exception cref="System.AggregateException">Thrown when a method call on the HttpClient throws an AggregateException</exception>
        public void GetAllFinancialTransactionPages()
        {
            int totalCount = 0;
            int fetchedCount = 0;
            int page = 0;
            do
            {
                page++;
                Trace.WriteLine($"Fetching page: {page}");
                var ftPage = GetFinancialTransactionPage(page);
                Trace.WriteLine($"Page read successfully");
                totalCount = ftPage.TotalCount; // This usually won't change unless the backend is adding new transactions during execution
                fetchedCount += ftPage.FinancialTransactions.Count;
                Trace.WriteLine($"ftPage.Page: {ftPage.Page}");
                Trace.WriteLine($"ftPage.TotalCount: {ftPage.TotalCount}");
                Trace.WriteLine($"totalCount: {totalCount}");
                Trace.WriteLine($"PageCount: {ftPage.FinancialTransactions.Count}");
                Trace.WriteLine($"fetchedCount: {fetchedCount}");
                Trace.WriteLine("Adding transactions in current page to Model");
                Model.AddFinancialTransactions(ftPage.FinancialTransactions);
                Trace.WriteLine("---------------------------------------------");
                Trace.WriteLine("");
            } while (fetchedCount < totalCount);
        }

        /// <summary>
        /// Calls the web service one time to retrieve one financial transaction page. This is implemented as a 
        /// syncronous call.
        /// </summary>
        /// <param name="page">the page number to retrieve</param>
        /// <exception cref="WebServiceException">Thrown when the web service returns a non-200 HTTP status code</exception>
        /// <exception cref="System.AggregateException">Thrown when a method call on the HttpClient throws an AggregateException</exception>
        public FinancialTransactionPage GetFinancialTransactionPage(int page)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var response = _client.GetAsync(String.Format(FinancialTransactionsEndpoint, page)).Result;
            FinancialTransactionPage ftPage = null;
            var responseContent = response.Content;
            // calling Result makes this synchronous
            string responseString = responseContent.ReadAsStringAsync().Result;
            Trace.WriteLine(responseString);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // HttpStatusCode.OK is code 200. Other codes in the 201-299 range are considered success
                // by HttpResponseMessage.IsSuccessStatusCode, but for the purposes of this
                // application, I am only considering 200 to be successful.
                ftPage = JsonSerializer.Deserialize<FinancialTransactionPage>(responseString);
            }
            else
            {
                throw new WebServiceException
                {
                    StatusCode = (int)response.StatusCode,
                    Content = responseString
                };
            }
            return ftPage;
        }
    }
}
