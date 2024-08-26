namespace Interview.Entity.History
{
    public class SearchHistory
    {
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string Query { get; set; }
        public DateTime Timestamp { get; set; }
        public List<SearchResult> SearchResults { get; set; }
    }
}
