namespace BenchRestClient
{
    /// <summary>
    /// Interface for a controller for the Bench transactions web service. It calls the web service,
    /// saves the results in the model and updates the view. It is the controller part of the
    /// model-view-controller (MVC) pattern.
    /// 
    /// Although this application could be implemented without declaring this interface,
    /// I chose to declare this interface to facilitate future testing because interfaces
    /// can be easily mocked or stubbed. Interfaces also allow for alternative implementations
    /// that can be dependency injected into other dependent objects.
    /// </summary>
    public interface IFinancialTransactionController
    {
        /// <summary>
        /// The endpoint for the Bench transactions web service.
        /// </summary>
        string FinancialTransactionsEndpoint { get; set; }
        /// <summary>
        /// The model portion of this model-view-controller triad.
        /// </summary>
        IFinancialTransactionModel Model { get; set; }
        /// <summary>
        /// The view portion of this model-view-controller triad.
        /// </summary>
        IFinancialTransactionView View { get; set; }
        /// <summary>
        /// Gets all financial transaction pages from the web service, puts them in the model, and displays the
        /// running daily balances in the view. Resulting exceptions are displayed in the view.
        /// </summary>   
        void DisplayRunningDailyBalances();
    }
}