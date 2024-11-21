using Knap.Infrastructure.ApiClient;
using Knap.Infrastructure.Dto;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace Knap.Tests.Unit
{
    /// <summary>
    /// Unit tests for the <see cref="CoinMarketCapApiWrapper"/> class.
    /// </summary>
    public class CoinMarketCapApiWrapperTests
    {
        private readonly List<string> _symbols = new List<string> { "USD", "EUR", "GBP" };
        private readonly string _baseCurrency = "BTC";

        [Fact]
        public async Task GetCurrencyRatesAsync_ReturnsCurrencyRateResponse()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var apiResponses = new Dictionary<string, CoinMarketCapDto>
            {
                {
                    "USD",
                    new CoinMarketCapDto
                    {
                        data =
                        [
                            new CoinMarketCapDateDto
                            {
                                quote = new Dictionary<string, CoinMarketCap>
                                {
                                    { "USD", new CoinMarketCap { price = 50000.12m } }
                                }
                            }
                        ]
                    }
                },
                {
                    "EUR",
                    new CoinMarketCapDto
                    {
                        data = [
                            new CoinMarketCapDateDto
                            {
                                quote = new Dictionary<string, CoinMarketCap>
                                {
                                    { "EUR", new CoinMarketCap { price = 45000.75m } }
                                }
                            }
                        ]
                    }
                },
                {
                    "GBP",
                    new CoinMarketCapDto
                    {
                        data = [
                            new CoinMarketCapDateDto
                            {
                                quote = new Dictionary<string, CoinMarketCap>
                                {
                                    { "GBP", new CoinMarketCap { price = 39000.30m } }
                                }
                            }
                        ]
                    }
                }
            };

            // Mock the HTTP responses for each symbol
            foreach (var symbol in _symbols)
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(apiResponses[symbol]))
                };

                mockMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(req => req.RequestUri != null && req.RequestUri.ToString().Contains($"convert={symbol}")),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(response);

            }

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(mockMessageHandler.Object)
          {
              BaseAddress = new Uri("https://api.coinmarketcap.com/v1/")
          };
            mockHttpClientFactory
                .Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var apiWrapper = new CoinMarketCapApiWrapper(mockHttpClientFactory.Object);

            // Act
            var result = await apiWrapper.GetCurrencyRatesAsync(_baseCurrency, _symbols);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(_baseCurrency, result.BaseCurrency);
            Assert.Equal(3, result.Rates.Count);
            Assert.Equal(50000.12m, result.Rates["USD"]);
            Assert.Equal(45000.75m, result.Rates["EUR"]);
            Assert.Equal(39000.30m, result.Rates["GBP"]);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_HandlesApiError()
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("API Error"));

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(mockMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.coinmarketcap.com/v1/")
            };
            mockHttpClientFactory
                .Setup(factory => factory.CreateClient("CoinMarketCapClient"))
                .Returns(httpClient);

            var apiWrapper = new CoinMarketCapApiWrapper(mockHttpClientFactory.Object);

            // Act
            var result = await apiWrapper.GetCurrencyRatesAsync(_baseCurrency, _symbols);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.Success);
            Assert.Empty(result.Rates);
        }      
      
    }
}
