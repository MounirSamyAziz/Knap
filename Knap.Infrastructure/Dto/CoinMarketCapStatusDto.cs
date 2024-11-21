namespace Knap.Infrastructure.Dto
{
    /// <summary>
    /// Represents the status metadata for a response from the CoinMarketCap API.
    /// </summary>
    public class CoinMarketCapStatusDto
    {
        /// <summary>
        /// Gets or sets the timestamp when the response was generated.
        /// </summary>
        public DateTime timestamp { get; set; }

        /// <summary>
        /// Gets or sets the error code from the API response, if any.
        /// </summary>
        public int error_code { get; set; }

        /// <summary>
        /// Gets or sets the error message associated with the response, if any.
        /// </summary>
        /// <remarks>
        /// This property is null when there is no error.
        /// </remarks>
        public object error_message { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time (in milliseconds) taken to generate the response.
        /// </summary>
        public int elapsed { get; set; }

        /// <summary>
        /// Gets or sets the number of API credits used for the request.
        /// </summary>
        public int credit_count { get; set; }

        /// <summary>
        /// Gets or sets any additional notices associated with the response.
        /// </summary>
        /// <remarks>
        /// This property is null when there are no notices.
        /// </remarks>
        public object notice { get; set; }
    }
}
