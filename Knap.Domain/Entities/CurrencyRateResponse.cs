using Newtonsoft.Json;

namespace Knap.Domain.Entities
{
    /// <summary>
    /// Represents a response containing currency rates and metadata.
    /// </summary>
    public class CurrencyRateResponse
    {
        /// <summary>
        /// Indicates whether the request for currency rates was successful.
        /// </summary>
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        /// <summary>
        /// The timestamp indicating when the currency rates were retrieved.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        public int Timestamp { get; set; }

        /// <summary>
        /// The base currency used for the rate calculations.
        /// </summary>
        [JsonProperty(PropertyName = "base")]
        public string BaseCurrency { get; set; }

        /// <summary>
        /// The date when the currency rates were retrieved, in string format.
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        /// <summary>
        /// A dictionary containing the currency rates, with the currency code as the key 
        /// and the rate as the value.
        /// </summary>
        [JsonProperty(PropertyName = "rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
