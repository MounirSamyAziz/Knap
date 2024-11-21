using Knap.Domain.Entities;

namespace Knap.Application.Contracts.Interfaces
{
    /// <summary>
    /// Interface for changing the base rate of a currency rate response.
    /// </summary>
    public interface IChangeBaseRate
    {
        /// <summary>
        /// Sets a new base currency for the given currency rate response.
        /// </summary>
        /// <param name="newBase">The new base currency code (e.g., "EUR").</param>
        /// <param name="currencyRateResponse">The current currency rate response to modify.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated currency rate response.</returns>
        Task<CurrencyRateResponse> SetBase(string newBase, CurrencyRateResponse currencyRateResponse);
    }
}
