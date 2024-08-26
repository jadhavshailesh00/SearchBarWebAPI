namespace Interview.Entity.Response
{
    /// <summary>
    /// Represents a standardized error response.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the error message explaining what went wrong.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets an optional error code to categorize the error.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets information on how to resolve the issue.
        /// </summary>
        public string Resolution { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier for this error instance.
        /// </summary>
        public string ErrorId { get; set; }
    }
}

