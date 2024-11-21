using Knap.Application.Contracts.Infrastructure;
using Knap.Domain.Entities;
using Knap.Infrastructure.Dto;
using Newtonsoft.Json;

namespace Knap.Infrastructure.ApiClient
{
    /// <summary>
    /// Wrapper for interacting with the CoinMarketCap API to fetch currency rates.
    /// </summary>
    public class CoinMarketCapApiWrapper : ICurrencyApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoinMarketCapApiWrapper"/> class.
        /// </summary>
        /// <param name="apiUrl">The base URL of the CoinMarketCap API.</param>
        /// <param name="apiKey">The API key for authenticating requests.</param>
        /// <param name="httpClient">An instance of <see cref="HttpClient"/> for making HTTP requests.</param>
        public CoinMarketCapApiWrapper(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        /// <summary>
        /// Fetches currency rates asynchronously from the CoinMarketCap API.
        /// </summary>
        /// <param name="baseCurrency">The base currency for conversion.</param>
        /// <param name="symbols">A list of target currency symbols to convert to.</param>
        /// <returns>
        /// A <see cref="CurrencyRateResponse"/> object containing the rates and metadata.
        /// </returns>
        /// <exception cref="ApplicationException">Thrown if there is an error fetching rates.</exception>
        public async Task<CurrencyRateResponse> GetCurrencyRatesAsync(string baseCurrency, List<string> symbols)
        {
            var result = new CurrencyRateResponse
            {
                Success = true,
                BaseCurrency = baseCurrency,
                Rates = []
            };

            try
            {
                var tasks = new List<Task<CoinMarketCapDto>>();



                // Start all API calls concurrently
                foreach (var symbol in symbols)
                {
                    tasks.Add(MakeAPICovertCallAsync(baseCurrency, symbol));
                }

                // Wait for all API calls to complete
                var taskResults = await Task.WhenAll(tasks);

                // Process the API results
                foreach (var taskResult in taskResults)
                {
                    var item = taskResult.data[0].quote.FirstOrDefault();
                    result.Rates.Add(
                        item.Key,
                        decimal.Round(item.Value.price.HasValue ? item.Value.price.Value : 0, 2, MidpointRounding.AwayFromZero)
                    );
                }

                return result;
            }
            catch (ApplicationException ex)
            {
                result.Success = false;
                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow as needed
                throw new ApplicationException("Error fetching currency rates", ex);
            }
        }

        /// <summary>
        /// Makes an asynchronous API call to the CoinMarketCap API to fetch currency conversion data.
        /// </summary>
        /// <param name="symbolFrom">The source currency symbol.</param>
        /// <param name="symbolTo">The target currency symbol.</param>
        /// <returns>
        /// A <see cref="CoinMarketCapDto"/> object containing the conversion data.
        /// </returns>
        private async Task<CoinMarketCapDto> MakeAPICovertCallAsync(string symbolFrom, string symbolTo)
        {
            try
            {
                // Use the factory to create the HttpClient for "CoinMarketCapClient"
                var httpClient = _httpClientFactory.CreateClient("CoinMarketCapClient");
                // Build the API request URL with query parameters
                var URL = new UriBuilder(httpClient.BaseAddress);
                var queryParams = new Dictionary<string, string>
                    {
                        { "symbol", symbolFrom },
                        { "amount", "1" },
                        { "convert", symbolTo }
                    };
                var queryString = new FormUrlEncodedContent(queryParams).ReadAsStringAsync().Result;

                URL.Query = queryString;

                // Make the request asynchronously
                HttpResponseMessage response = await httpClient.GetAsync(URL.ToString());
                response.EnsureSuccessStatusCode();

                // Parse the response content into the DTO
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<CoinMarketCapDto>(content);
                return result;
            }
            catch (Exception ex)
            {
                // Handle exceptions or rethrow as needed
                throw new ApplicationException("Error fetching currency rates", ex);
            }
        }
    }
}
