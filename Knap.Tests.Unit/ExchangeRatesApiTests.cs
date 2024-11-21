using Knap.Application.Contracts.Interfaces;
using Knap.Domain.Entities;
using Knap.Infrastructure.ApiClient;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace Knap.Tests.Unit
{
    /// <summary>
    /// Unit tests for the <see cref="ExchangeRatesApi"/> class.
    /// </summary>
    public class ExchangeRatesApiTests
    {
        private readonly List<string> _symbols = new List<string> { "USD", "EUR", "BRL", "GBP", "AUD" };

        [Fact]
        public async Task GetCurrencyRatesAsync_ReturnsCurrencyRateResponse()
        {
            // Arrange: Mock the message handler and HttpClient
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var expectedApiResponse = new CurrencyRateResponse
            {
                BaseCurrency = "EUR",
                Success = true,
                Timestamp = 1732054263,
                Date = "2024-11-19",
                Rates = new Dictionary<string, decimal>
                {
                    { "USD", 1.055742m },
                    { "EUR", 1m },
                    { "BRL", 6.104929m },
                    { "GBP", 0.83335m },
                    { "AUD", 1.621026m },
                    { "BTC", 0.000011332407m }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedApiResponse))
            };

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(mockMessageHandler.Object) { BaseAddress = new Uri("https://api.exchangerate-api.com/v4/latest/") };
            mockHttpClientFactory
                .Setup(factory => factory.CreateClient("ExchangeRatesClient"))
                .Returns(httpClient);

            // Mock the IChangeBaseRate
            var expectedResult = new CurrencyRateResponse
            {
                BaseCurrency = "BTC",
                Rates = new Dictionary<string, decimal>
                {
                    { "EUR", 88242.51m },
                    { "USD", 93161.32m },
                    { "BRL", 538714.24m },
                    { "GBP", 73536.89m },
                    { "AUD", 143043.40m }
                }
            };

            var mockChangeBaseRate = new Mock<IChangeBaseRate>();
            mockChangeBaseRate
                .Setup(x => x.SetBase(It.IsAny<string>(), It.IsAny<CurrencyRateResponse>()))
                .ReturnsAsync(expectedResult);

            // Create the ExchangeRatesApi instance
            var exchangeRatesApi = new ExchangeRatesApi(
                mockHttpClientFactory.Object,
                "fake_access_key",
                mockChangeBaseRate.Object);

            // Act
            var result = await exchangeRatesApi.GetCurrencyRatesAsync("BTC", _symbols);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("BTC", result.BaseCurrency);
            Assert.Equal(5, result.Rates.Count);
            Assert.Equal(88242.51m, result.Rates["EUR"]);
            Assert.Equal(93161.32m, result.Rates["USD"]);
            Assert.Equal(538714.24m, result.Rates["BRL"]);
            Assert.Equal(73536.89m, result.Rates["GBP"]);
            Assert.Equal(143043.40m, result.Rates["AUD"]);
        }

        [Fact]
        public async Task GetCurrencyRatesAsync_ThrowsException_WhenApiFails()
        {
            // Arrange: Mock the message handler to throw an exception
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("API Error"));

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var httpClient = new HttpClient(mockMessageHandler.Object);
            mockHttpClientFactory
                .Setup(factory => factory.CreateClient("ExchangeRatesClient"))
                .Returns(httpClient);

            var mockChangeBaseRate = new Mock<IChangeBaseRate>();
            var exchangeRatesApi = new ExchangeRatesApi(
                mockHttpClientFactory.Object,
                "fake_access_key",
                mockChangeBaseRate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => exchangeRatesApi.GetCurrencyRatesAsync("USD", _symbols));
        }
    }
}
