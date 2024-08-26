using Interview.Entity.History;
using Interview.Entity.Response;

namespace Interview.Service.Search
{
    public interface ISearchService
    {
        SearchResponse SearchDataByID(string UserID, string id);
        List<SearchResponse> SearchData(string UserID, string query, string filter, string sort);

        public List<SearchHistory> GetSearchHistoryAsync(string userId);
    }
}
