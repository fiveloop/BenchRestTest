# BenchRestTest
This is a C# implementation of the [Bench.co Rest Test](https://resttest.bench.co/).

## How to Install and Run the Program from Pre-Compiled Binaries

1. Download the latest release from the [Releases](https://github.com/fiveloop/BenchRestTest/releases) page. The releases follow the naming convention BenchRestTest-*yyyy-mm-dd*-*platform*.zip. For example, BenchRestTest-2022-02-07-win-x64.zip.
2. Unzip the release to any arbitrary folder.
3. Open a Windows command prompt and browse to that folder.
4. Enter **BenchRestClient.exe** at the command prompt to run the program in that folder.

## Project Dependencies

This project targets Microsoft .NET 6.0. If you don't have it installed already, you can download it here: [Download .NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

## How to Build The Program Yourself

This solution was created with [Microsoft Visual Studio Community 2022](https://visualstudio.microsoft.com/vs/community/). If you want to build the executable yourself, you can do the following:
1. Install Microsoft Visual Studio Community 2022.
2. Use the NuGet Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console) to install the ConfigurationManager package from NuGet with the command **Install-Package System.Configuration.ConfigurationManager**
3. Use the NuGet Package Manager Console in Visual Studio (Tools > NuGet Package Manager > Package Manager Console) to install the Moq package from NuGet with the command **Install-Package Moq**
4. Download this solution's source code from this Github repository.
5. Open the solution file **BenchRestTest.sln** in Visual Studio.
6. Click Build > Rebuild Solution.
7. The BenchRestClient.exe will be created in the either the bin\Debug or bin\Release folder, depending on whether you selected the Debug or Release configuration.
8. After the exe is built, you can run it within Visual Studio by clicking either **Debug > Start Debugging** or **Debug > Start Without Debugging**.

## Application Configuration

The program can be configured by editing the application configuration file. If you are running the program from pre-compiled binaries, the application configuration file is **BenchRestClient.dll.config**. If you are running the program from Visual Studio, the application configuration file is **app.config**.

Here are the configurable settings:
- `PrintTraceMessagesToConsole` - Set to `true` to dump execution debug trace messages to the console or `false` to turn off. Default is `false`.
- `FinancialTransactionsEndpoint` - This is the URL of the endpoint to the Bench Rest Test transactions web service. Default value is `https://resttest.bench.co/transactions/{page}.json`. My guess is that endpoint is unlikely to change in the future, but if it does, you can at least change it in the application configuration file without having to re-compile this program.

## Example Output

Based on the results returned from the Bench Rest Test transactions web service as of February 7th, 2022, here is the example output of this program when the PrintTraceMessagesToConsole setting is set to `false` (default):<br />
2013-12-12 -227.35<br />
2013-12-13 -1456.93<br />
2013-12-15 -1462.32<br />
2013-12-16 -6037.85<br />
2013-12-17 4648.43<br />
2013-12-18 2807.14<br />
2013-12-19 22560.45<br />
2013-12-20 18505.85<br />
2013-12-21 18487.87<br />
2013-12-22 18377.16<br />

**Note:** While it certainly would have been very easy to make the output "fancier", the above example output follows the *exact* output requirements specified on the [Bench.co Rest Test](https://resttest.bench.co/) page. No more. No less.

## Unit Testing this Program

The source code contains a fairly comprehensive suite of unit tests implemented with [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest) and [Moq](https://github.com/moq/moq). The suite of tests can be run from within Visual Studio by right-clicking on the **BenchRestClientTest** project and clicking **Run Tests**. Special thanks go to [Makolyte](https://makolyte.com/) for demonstrating some [unit testing techniques](https://makolyte.com/csharp-how-to-unit-test-code-that-uses-httpclient/) for testing code that depends on [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-6.0).

## What are the Limitations and Tradeoffs of this Program? What could be better?

- Although Microsoft's [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-6.0) class supports [asynchronous programming](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/) with the `async` and `await` keywords, I chose to use synchronous programming to simplify the coding and testing of this program. In a future release of this program, I may switch to asynchronous programming, which would allow multiple [Tasks](https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-6.0) to be run asynchronously and provide the ability to do [Task cancellation](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-cancellation). However, since only one web service call is currently being made by this program, I deemed that this extra complication was not necessary in the first implementation.
- I used the [HttpContent.ReadAsStringAsync](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent.readasstringasync?view=net-6.0) method to read the content from the web service response. While it works, I could possibly have [achieved faster performance and used less memory](https://johnthiriet.com/efficient-api-calls/) if I had used the [HttpContent.ReadAsStreamAsync](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpcontent.readasstreamasync?view=net-6.0) method instead. In a future release of this program, I may make that change. **That would be an especially important change to make if the web service itself was ever changed to return several thousand transactions instead of just the few that it currently returns.**
- The `FinancialTransactionModel` class stores *all* returned transactions in the `FinancialTransactions` property. Strictly speaking, that is not necessary because I calculate and store the summarized daily transaction totals in the `DailyTotals` property as each page of transactions is retrieved from the web service. I could reduce memory by removing the `FinancialTransactions` property. However, I decided to keep the `FinancialTransactions` property to facilitate future program features such as displaying all transactions for a given day.

## A Few Notes About the Design

- I used the [model-view-controller pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller) in my design to separate concerns to make the program easier to maintain.
- Before I created my model, view and controller classes, I first created interfaces for them. Creating interfaces has advantages over using just classes:
  - Interfaces can be easily [stubbed or mocked](https://circleci.com/blog/how-to-test-software-part-i-mocking-stubbing-and-contract-testing/) to facilite unit testing.
  - Interfaces allow different implementations of model, view and controller classes to be [dependency injected](https://en.wikipedia.org/wiki/Dependency_injection) into the MVC triad without each component being aware of the specific implementation. This makes the program easier to maintain and test.
