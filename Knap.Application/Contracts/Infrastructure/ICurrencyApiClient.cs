using Knap.Domain.Entities;

namespace Knap.Application.Contracts.Infrastructure
{
    /// <summary>
    /// Interface for a client that retrieves currency rates from an external API.
    /// </summary>
    public interface ICurrencyApiClient
    {
        /// <summary>
        /// Retrieves currency rates based on the provided base currency and a list of target symbols.
        /// </summary>
        /// <param name="baseCurrency">The base currency code (e.g., "USD").</param>
        /// <param name="symbols">A list of target currency codes to get exchange rates for (e.g., "EUR", "GBP").</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the currency rate response.</returns>
        Task<CurrencyRateResponse> GetCurrencyRatesAsync(string baseCurrency, List<string> symbols);
    }
}
