namespace Knap.Mvc.Models
{
    /// <summary>
    /// Represents a model for displaying error information in the application.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the current request.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indicates whether the <see cref="RequestId"/> should be displayed.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
