using Knap.Application.Contracts.Infrastructure;
using Knap.Application.Contracts.Interfaces;
using Knap.Domain.Entities;
using Newtonsoft.Json;

namespace Knap.Infrastructure.ApiClient
{
    /// <summary>
    /// Implementation of the currency API client for the ExchangeRates API.
    /// </summary>
    public class ExchangeRatesApi : ICurrencyApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _accessKey;
        private readonly IChangeBaseRate _changeBaseRate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRatesApi"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to make requests to the API.</param>
        /// <param name="apiUrl">The base URL of the ExchangeRates API.</param>
        /// <param name="accessKey">The API access key for authentication.</param>
        /// <param name="changeBaseRate">The service for changing the base currency rate.</param>
        public ExchangeRatesApi(IHttpClientFactory httpClientFactory, string accessKey, IChangeBaseRate changeBaseRate)
        {
            _httpClientFactory = httpClientFactory;
            _accessKey = accessKey;
            _changeBaseRate = changeBaseRate;
        }

        /// <summary>
        /// Retrieves currency rates for the specified base currency and target symbols from the ExchangeRates API.
        /// </summary>
        /// <param name="baseCurrency">The base currency for rate conversions.</param>
        /// <param name="symbols">A list of target currency symbols to retrieve rates for.</param>
        /// <returns>
        /// A <see cref="Task"/> that resolves to a <see cref="CurrencyRateResponse"/> containing the currency rates.
        /// </returns>
        /// <exception cref="ApplicationException">Thrown when an error occurs while fetching currency rates.</exception>
        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync(string baseCurrency, List<string> symbols)
        {
            try
            {
                // Use the factory to create the HttpClient for "ExchangeRatesClient"
                using var httpClient = _httpClientFactory.CreateClient("ExchangeRatesClient");
                // Construct the API URL with the access key and target symbols
                var response = await httpClient.GetAsync(
                    $"{httpClient.BaseAddress}?access_key={_accessKey}&symbols=USD,EUR,BRL,GBP,AUD,{baseCurrency}");
                response.EnsureSuccessStatusCode(); // Ensure the HTTP request was successful

                // Read the response content as a JSON string
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON content into a CurrencyRateResponse object
                var result = JsonConvert.DeserializeObject<CurrencyRateResponse>(content);

                // Adjust the base currency rate using the IChangeBaseRate dependency
                return await _changeBaseRate.SetBase(baseCurrency, result);
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow as needed
                throw new ApplicationException("Error fetching currency rates", ex);
            }
        }
    }
}
