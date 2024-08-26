namespace Interview.Entity.History
{
    public class SearchResult
    {
        public int ResultId { get; set; }
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string ResultData { get; set; }
        public int ResultRank { get; set; }
        public DateTime RetrievedAt { get; set; }
    }
}
