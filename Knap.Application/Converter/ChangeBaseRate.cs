using Knap.Application.Contracts.Interfaces;
using Knap.Domain.Entities;

namespace Knap.Application.Converter
{
    /// <summary>
    /// Implements functionality to change the base rate of a currency.
    /// </summary>
    public class ChangeBaseRate : IChangeBaseRate
    {
        /// <summary>
        /// Sets a new base currency and recalculates the currency rates accordingly.
        /// </summary>
        /// <param name="newBase">The new base currency to set.</param>
        /// <param name="currencyRateResponse">The current currency rate response containing rates and metadata.</param>
        /// <returns>
        /// A <see cref="CurrencyRateResponse"/> object with the new base currency and updated rates.
        /// </returns>
        /// <exception cref="ApplicationException">Thrown when the specified new base currency is not found in the rates.</exception>
        public Task<CurrencyRateResponse> SetBase(string newBase, CurrencyRateResponse currencyRateResponse)
        {
            // Check if the new base currency exists in the rates
            if (!currencyRateResponse.Rates.Any(p => p.Key == newBase))
                throw new ApplicationException("Could not find this cryptocurrency");

            // Create a new response object with updated base currency and rates
            CurrencyRateResponse result = new()
            {
                BaseCurrency = newBase,
                Date = currencyRateResponse.Date,
                Timestamp = currencyRateResponse.Timestamp,
                Success = currencyRateResponse.Success,
                Rates = []
            };

            // Get the rate value for the new base currency
            var newBaseValue = currencyRateResponse.Rates.FirstOrDefault(p => p.Key == newBase);
            decimal currentRateValue = 1 / newBaseValue.Value;

            // Add the current base currency rate to the result
            result.Rates.Add(currencyRateResponse.BaseCurrency, decimal.Round(currentRateValue, 2, MidpointRounding.AwayFromZero));

            // Recalculate and add rates for other currencies
            foreach (var item in currencyRateResponse.Rates.Where(p => p.Key != newBase && p.Key != currencyRateResponse.BaseCurrency))
            {
                result.Rates.Add(
                    item.Key,
                    decimal.Round(item.Value * currentRateValue, 2, MidpointRounding.AwayFromZero)
                );
            }

            return Task.FromResult(result);
        }
    }
}
