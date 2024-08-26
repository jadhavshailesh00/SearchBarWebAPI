using Interview.Entity.Response;
using System.Data.SqlClient;

namespace Interview.Repository
{
    public class Repository : IRepository
    {
        private readonly List<SearchResponse> _data = new List<SearchResponse>();
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SearchResponse SearchDataByID(string ID)
        {

            var result = new SearchResponse();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ContentItems WHERE ID = @ID ORDER BY Date DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _data = new SearchResponse
                            {
                                ID = reader.GetString(0),
                                Category = reader.GetString(1),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Description = reader.GetString(3),
                                Title = reader.GetString(4),
                            };
                            result = _data;
                        }
                    }

                }

            }

            if (result == null)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        public List<SearchResponse> SearchData(string query, string filter, string sort)
        {

            var searchData = new List<SearchResponse>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM ContentItems WHERE Title LIKE @query OR Description LIKE @query OR Category LIKE @query ORDER BY Date DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@query", "%" + query + "%");

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _data = new SearchResponse
                            {
                                ID = reader.GetString(reader.GetOrdinal("ID")),
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                            };
                            searchData.Add(_data);
                        }
                    }
                }
            }
            if (searchData.Count() == 0)
            {
                return null;
            }
            return searchData.ToList();
        }
    }
}
