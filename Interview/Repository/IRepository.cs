using Interview.Entity.Response;

namespace Interview.Repository
{
    public interface IRepository
    {
        SearchResponse SearchDataByID(string id);
        List<SearchResponse> SearchData(string query, string filter, string sort);
    }
}
