using Knap.Application.Contracts.Infrastructure;
using Knap.Domain.Entities;
using Knap.Infrastructure.ApiClient;
using Knap.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Knap.Tests.Integration
{
    public class HomeControllerTest : IClassFixture<KnapMvcServerFactory>, IDisposable
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly ICurrencyApiClient _coinMarketCapApiWrapper;
        private readonly ICurrencyApiClient _exchangeRatesApiWrapper;

        public HomeControllerTest(KnapMvcServerFactory factory)
        {
            scope = factory.Services.CreateScope();
            _mockLogger = new Mock<ILogger<HomeController>>();
            _coinMarketCapApiWrapper = factory.Services.GetServices<ICurrencyApiClient>().OfType< CoinMarketCapApiWrapper >().FirstOrDefault();
            _exchangeRatesApiWrapper = factory.Services.GetServices<ICurrencyApiClient>().OfType<ExchangeRatesApi>().FirstOrDefault(); 
        }
        protected readonly HttpClient client;
        protected readonly IServiceScope scope;
        public void Dispose()
        {
            scope.Dispose();
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCurrencyRates_WhenBaseCurrencyIsProvided()
        {
            // Arrange
            var baseCurrency = "USD";
            var symbols = new List<string> { "USD", "EUR", "BRL", "GBP", "AUD" };

            var mockCoinMarketCapResponse = new CurrencyRateResponse { BaseCurrency = baseCurrency };
            var mockExchangeRatesResponse = new CurrencyRateResponse { BaseCurrency = baseCurrency };


            var services = new List<ICurrencyApiClient> { _coinMarketCapApiWrapper, _exchangeRatesApiWrapper };
            var controller = new HomeController(_mockLogger.Object, services);

            // Act
            var result = await controller.Index(baseCurrency) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<Dictionary<string, CurrencyRateResponse>>(result.Model);
            Assert.Equal(2, model.Count);
            Assert.Contains("https://coinmarketcap.com/api", model.Keys);
            Assert.Contains("https://exchangeratesapi.io", model.Keys);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithEmptyDictionary_WhenBaseCurrencyIsNotProvided()
        {
            // Arrange
            var baseCurrency = string.Empty;
            var services = new List<ICurrencyApiClient> { _coinMarketCapApiWrapper, _exchangeRatesApiWrapper };
            var controller = new HomeController(_mockLogger.Object, services);

            // Act
            var result = await controller.Index(baseCurrency) as ViewResult;

            // Assert
            Assert.NotNull(result);
            var model = Assert.IsAssignableFrom<Dictionary<string, CurrencyRateResponse>>(result.Model);
            Assert.Empty(model);
        }
    }

}
     
