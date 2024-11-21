using FluentAssertions;
using Knap.Application.Converter;
using Knap.Domain.Entities;

namespace Knap.Tests.Unit
{
    /// <summary>
    /// Unit tests for the <see cref="ChangeBaseRate"/> class.
    /// </summary>
    public class ChangeBaseRateTest
    {
        /// <summary>
        /// Tests the <see cref="ChangeBaseRate.SetBase"/> method with a base currency that is not a cryptocurrency.
        /// </summary>
        [Fact]
        public async void TestWithoutCryptoCurrency()
        {
            // Arrange: Create an instance of ChangeBaseRate and define the input and expected output.
            var changeBaseRate = new ChangeBaseRate();
            var currencyRateResponse = new CurrencyRateResponse()
            {
                BaseCurrency = "EUR",
                Rates = new Dictionary<string, decimal>
                {
                    { "EGP", 52.495587m },
                    { "USD", 1.059586m }
                }
            };
            var expectedResult = new CurrencyRateResponse()
            {
                BaseCurrency = "USD",
                Rates = new Dictionary<string, decimal>
                {
                    { "EGP", 49.54m },
                    { "EUR", 0.94m }
                }
            };

            // Act: Call the SetBase method to recalculate the rates.
            var result = await changeBaseRate.SetBase("USD", currencyRateResponse);

            // Assert: Verify that the result matches the expected output.
            expectedResult.Rates.Should().BeEquivalentTo(result.Rates);
        }

        /// <summary>
        /// Tests the <see cref="ChangeBaseRate.SetBase"/> method with a base currency that is a cryptocurrency.
        /// </summary>
        [Fact]
        public async void TestWithCryptoCurrency()
        {
            // Arrange: Create an instance of ChangeBaseRate and define the input and expected output.
            var changeBaseRate = new ChangeBaseRate();
            var currencyRateResponse = new CurrencyRateResponse
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
            var expectedResult = new CurrencyRateResponse()
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

            // Act: Call the SetBase method to recalculate the rates.
            var result = await changeBaseRate.SetBase("BTC", currencyRateResponse);

            // Assert: Verify that the base currency and rates match the expected output.
            expectedResult.BaseCurrency.Should().BeEquivalentTo(result.BaseCurrency);
            expectedResult.Rates.Should().BeEquivalentTo(result.Rates);
        }
    }
}
