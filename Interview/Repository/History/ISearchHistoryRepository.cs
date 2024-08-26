using Interview.Entity.History;
using Interview.Entity.Response;

namespace Interview.Repository.Search
{
    public interface ISearchHistoryRepository
    {
        public List<SearchHistory> GetSearchHistoryAsync(string UserID);
        public int SaveSearchData(string UserID, string query, string Filter, string Sort);
        public int SaveSearchDataByID(string UserID, string ID);

        public int SaveSearchHistroy(string searchId, SearchResponse results);
    }
}
