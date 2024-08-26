using Interview.Entity.History;
using Interview.Entity.Response;
using Interview.Repository;
using Interview.Repository.Search;

namespace Interview.Service.Search
{
    public class SearchService : ISearchService
    {
        private readonly IRepository _searchResultRepository;
        private ISearchHistoryRepository _SearchHistoryRepository;

        public SearchService(IRepository searchResultRepository, ISearchHistoryRepository searchHistoryRepository)
        {
            _searchResultRepository = searchResultRepository;
            _SearchHistoryRepository = searchHistoryRepository;
        }


        public SearchResponse SearchDataByID(string UserID, string query)
        {
            var data = _searchResultRepository.SearchDataByID(query);
            ExecuteSaveSearchDataByID(UserID, query);
            ExecuteSaveSearchHistroy(UserID, data);
            return data;
        }

        public List<SearchResponse> SearchData(string UserID, string query, string filter, string sort)
        {
            var resultData = _searchResultRepository.SearchData(query, filter, sort);
            if (resultData != null)
            {

                resultData = ApplyFilter(resultData, filter);
                resultData = ApplySort(resultData, sort);

                int data = ExecuteSaveSearchData(UserID, query, filter, sort);
                foreach (var currentData in resultData)
                {
                    ExecuteSaveSearchHistroy(UserID, currentData);
                }
            }
            return resultData;
        }

        private List<SearchResponse> ApplyFilter(List<SearchResponse> resultData, string filter)
        {
            if (filter == "recent")
            {
                return resultData.Where(item => item.Date >= DateTime.UtcNow.AddDays(-30) && item.Date <= DateTime.UtcNow).ToList();;
            }
            return resultData;
        }

        private List<SearchResponse> ApplySort(List<SearchResponse> resultData, string sort)
        {
            return sort switch
            {
                "ID" => resultData.OrderByDescending(item => item.ID).ToList(),
                "Title" => resultData.OrderBy(item => item.Title).ToList(),
                "Description" => resultData.OrderBy(item => item.Description).ToList(),
                "Category" => resultData.OrderBy(item => item.Category).ToList(),
                "Date" => resultData.OrderBy(item => item.Date).ToList(),
                _ => resultData,
            };
        }

        private int ExecuteSaveSearchData(string UserID, string query, string Filter, string Sort)
        {
            int searchId = _SearchHistoryRepository.SaveSearchData(UserID, query, Filter, Sort);
            return searchId;
        }

        private int ExecuteSaveSearchDataByID(string UserID, string query)
        {
            int searchId = _SearchHistoryRepository.SaveSearchDataByID(UserID, query);
            return searchId;
        }

        private int ExecuteSaveSearchHistroy(string UserID, SearchResponse result)
        {
            int searchId = _SearchHistoryRepository.SaveSearchHistroy(UserID, result);
            return searchId;
        }

        public List<SearchHistory> GetSearchHistoryAsync(string userId)
        {
            return _SearchHistoryRepository.GetSearchHistoryAsync(userId);
        }


    }
}
