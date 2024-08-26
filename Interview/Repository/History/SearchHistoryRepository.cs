using Interview.Entity.History;
using Interview.Entity.Response;
using System.Data.SqlClient;

namespace Interview.Repository.Search
{
    public class SearchHistoryRepository : ISearchHistoryRepository
    {
        private readonly string _connectionString;

        public SearchHistoryRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<SearchHistory> GetSearchHistoryAsync(string UserID)
        {
            var history = new List<SearchHistory>();
            var SearchList = new List<int>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM SearchHistory WHERE UserId = @UserId ORDER BY Timestamp DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var searchHistory = new SearchHistory
                            {
                                SearchId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                Query = reader.GetString(2),
                                Timestamp = reader.GetDateTime(3)
                            };
                            SearchList.Add(searchHistory.SearchId);
                            history.Add(searchHistory);
                        }
                    }
                }
                foreach (int searchid in SearchList)
                {
                    var searchHistory = history.Find(sh => sh.SearchId.Equals(searchid)); 
                    if (searchHistory != null)
                    {
                        searchHistory.SearchResults = GetSearchResultsAsync(searchid, conn); 
                    }
                }
                return history;
            }
        }

        private List<SearchResult> GetSearchResultsAsync(int searchId, SqlConnection conn)
        {
            var results = new List<SearchResult>();

            using (SqlCommand cmd = new SqlCommand("SELECT * FROM SearchResults WHERE SearchId = @SearchId", conn))
            {
                cmd.Parameters.AddWithValue("@SearchId", searchId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new SearchResult
                        {
                            ResultId = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            SearchId = reader.GetInt32(2),
                            ResultData = reader.GetString(3),
                            ResultRank = reader.GetInt32(4),
                            RetrievedAt = reader.GetDateTime(5)
                        });
                    }
                }
            }

            return results;
        }

        public int SaveSearchData(string UserID, string query, string Filter, string Sort)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO SearchHistory (UserId, Query, Timestamp) OUTPUT INSERTED.SearchId VALUES (@UserId, @Query,@Timestamp)", conn))
                {
                    string tempquery = "query=" + query + " Filter=" + Filter + " Sort=" + Sort;
                    cmd.Parameters.AddWithValue("@UserId", UserID);
                    cmd.Parameters.AddWithValue("@Query", tempquery);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);

                    return (int)cmd.ExecuteScalar();
                }
            }
        }
        public int SaveSearchDataByID(string UserID, string ID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO SearchHistory (UserId, Query, Timestamp) OUTPUT INSERTED.SearchId VALUES (@UserId, @Query, @Timestamp)", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserID);
                    cmd.Parameters.AddWithValue("@Query", "query=" + ID);
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);
                    return (int)cmd.ExecuteScalar();
                }
            }
        }


        public int SaveSearchHistroy(string UserID, SearchResponse results)
        {
            var searchdata = GetLatestSearchHistory();
            string ResultData = "Title=" + results.Title + "    Category=" + results.Category + "   Description=" + results.Description;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO SearchResults (UserId, SearchId,ResultData, ResultRank, RetrievedAt) VALUES (@UserId, @SearchId, @ResultData,@ResultRank, @RetrievedAt)", conn))
                {
                    cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(UserID));
                    cmd.Parameters.AddWithValue("@SearchId", searchdata.SearchId);
                    cmd.Parameters.AddWithValue("@ResultData", ResultData);
                    cmd.Parameters.AddWithValue("@ResultRank", 1);
                    cmd.Parameters.AddWithValue("@RetrievedAt", System.DateTime.Now);
                    return cmd.ExecuteNonQuery();
                }
            }
            return 0;
        }

        private SearchHistory GetLatestSearchHistory()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM SearchHistory ORDER BY Timestamp DESC", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SearchHistory
                            {
                                SearchId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                Query = reader.GetString(2),
                                Timestamp = reader.GetDateTime(3)

                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

    }
}
