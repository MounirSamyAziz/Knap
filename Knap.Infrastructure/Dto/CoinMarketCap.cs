namespace Knap.Infrastructure.Dto
{
    /// <summary>
    /// Represents the detailed data for a currency retrieved from the CoinMarketCap API.
    /// </summary>
    public class CoinMarketCap
    {
        /// <summary>
        /// Gets or sets the current price of the currency.
        /// </summary>
        public decimal? price { get; set; }

        /// <summary>
        /// Gets or sets the last updated timestamp for the currency data.
        /// </summary>
        public DateTime last_updated { get; set; }
    }
}
