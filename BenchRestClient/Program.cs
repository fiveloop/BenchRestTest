using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BenchRestClient
{
    /// <summary>
    /// The main entry point into this application. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            IFinancialTransactionController controller = new FinancialTransactionController(new HttpClient());
            IFinancialTransactionModel model = new FinancialTransactionModel();
            controller.Model = model;
            FinancialTransactionView view = new FinancialTransactionView();
            controller.View = view;
            ConfigureApplication(controller);
            controller.DisplayRunningDailyBalances();
        }

        static void ConfigureApplication(IFinancialTransactionController controller)
        {
            Trace.WriteLine("Configuring controller");
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;

                string printTraceMessagesToConsole = ReadSetting(
                    appSettings, "PrintTraceMessagesToConsole", "false");
                if (printTraceMessagesToConsole == "true")
                {
                    Trace.Listeners.Add(new ConsoleTraceListener());
                }

                string financialTransactionsEndpoint = ReadSetting(
                    appSettings, "FinancialTransactionsEndpoint", FinancialTransactionController.DefaultFinancialTransactionsEndpoint);
                Trace.WriteLine($"Setting FinancialTransactionsEndpoint to: {financialTransactionsEndpoint}");
                controller.FinancialTransactionsEndpoint = financialTransactionsEndpoint;
            }
            catch (ConfigurationErrorsException ceException)
            {
                string errorMessage = "Error reading app settings. Using default values. Error details: " + ceException.Message;
                Trace.WriteLine(errorMessage);
            }
        }

        static string ReadSetting(NameValueCollection appSettings, string key, string defaultValue)  
        {
            string result = null;
            try  
            {  
                result = appSettings[key];
            }  
            catch (ConfigurationErrorsException ceException)  
            {
                string errorMessage = $"Error reading app setting {key}. Using default value. Error details: " + ceException.Message;
                Console.WriteLine(errorMessage);
            }
            return result ?? defaultValue;
        }  
    }
}
