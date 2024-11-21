namespace Knap.Infrastructure.Dto;

/// <summary>
/// Represents the detailed data for a specific currency entry retrieved from the CoinMarketCap API.
/// </summary>
public class CoinMarketCapDateDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the currency.
    /// </summary>
    public int id { get; set; }

    /// <summary>
    /// Gets or sets the symbol of the currency (e.g., USD, BTC).
    /// </summary>
    public string symbol { get; set; }

    /// <summary>
    /// Gets or sets the name of the currency (e.g., "United States Dollar").
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// Gets or sets the amount of the currency for which the rate is being retrieved.
    /// </summary>
    public int amount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the last update for the currency data.
    /// </summary>
    public DateTime last_updated { get; set; }

    /// <summary>
    /// Gets or sets the quotes for the currency, containing rates against other currencies.
    /// </summary>
    /// <remarks>
    /// The key represents the target currency symbol, and the value is a <see cref="CoinMarketCap"/> object with rate details.
    /// </remarks>
    public Dictionary<string, CoinMarketCap> quote { get; set; }
}
