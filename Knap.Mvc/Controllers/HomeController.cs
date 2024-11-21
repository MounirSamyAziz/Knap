using Knap.Application.Contracts.Infrastructure;
using Knap.Domain.Entities;
using Knap.Infrastructure.ApiClient;
using Knap.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Knap.Mvc.Controllers
{
    /// <summary>
    /// Controller for handling home page requests and currency rate operations.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ICurrencyApiClient _coinMarketCapApiWrapper;
        private readonly ICurrencyApiClient _exchangeRatesApiWrapper;

        /// <summary>
        /// A list of currency symbols for which rates will be fetched.
        /// </summary>
        private readonly List<string> _symbols = ["USD", "EUR", "BRL", "GBP", "AUD"];

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging events.</param>
        /// <param name="myServices">A collection of <see cref="ICurrencyApiClient"/> implementations to interact with APIs.</param>
        public HomeController( IEnumerable<ICurrencyApiClient> myServices)
        {
            _coinMarketCapApiWrapper = myServices.OfType<CoinMarketCapApiWrapper>().FirstOrDefault();
            _exchangeRatesApiWrapper = myServices.OfType<ExchangeRatesApi>().FirstOrDefault();
        }

        /// <summary>
        /// The main index action for the home page. Fetches currency rates from APIs and passes the data to the view.
        /// </summary>
        /// <param name="baseCurrency">The base currency for which rates will be fetched.</param>
        /// <returns>A <see cref="Task{IActionResult}"/> that represents the asynchronous operation, returning a view with the currency rates data.</returns>
        public async Task<IActionResult> Index([FromForm] string baseCurrency )
        {
            var result = new Dictionary<string, CurrencyRateResponse>();
            if (!string.IsNullOrWhiteSpace(baseCurrency))
            {
                // Fetch rates from the ExchangeRates API
                var resultexchangeRates = await _exchangeRatesApiWrapper.GetCurrencyRatesAsync(baseCurrency, _symbols);

                // Fetch rates from the CoinMarketCap API
                var resultcoinMarketCap = await _coinMarketCapApiWrapper.GetCurrencyRatesAsync(baseCurrency, _symbols);

                // Add the results from both APIs to the result dictionary
                result.Add("https://coinmarketcap.com/api", resultcoinMarketCap);
                result.Add("https://exchangeratesapi.io", resultexchangeRates);
            }

            // Pass the result to the view
            return View(result);
        }

        /// <summary>
        /// Handles errors by returning an error view.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the error view with the current request ID.
        /// </returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
