namespace Interview.Entity.Response
{
    /// <summary>
    /// Represents the response returned from a search operation.
    /// </summary>
    public class SearchResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier for the search result.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the title of the search result.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the search result.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the category of the search result.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the date when the search result was created or last updated.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
