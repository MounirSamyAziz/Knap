namespace Knap.Infrastructure.Dto
{
    /// <summary>
    /// Represents the data transfer object (DTO) for the CoinMarketCap API response.
    /// </summary>
    public class CoinMarketCapDto
    {
        /// <summary>
        /// Gets or sets the status information for the CoinMarketCap API response.
        /// </summary>
        public CoinMarketCapStatusDto status { get; set; }

        /// <summary>
        /// Gets or sets the array of data entries returned by the CoinMarketCap API.
        /// </summary>
        public CoinMarketCapDateDto[] data { get; set; }
    }
}
