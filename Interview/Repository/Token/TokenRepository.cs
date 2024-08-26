using Interview.Model;
using System.Data.SqlClient;

namespace Interview.Repository.Token
{
    public class TokenRepository : ITokenRepository
    {
        private readonly string _connectionString;

        public TokenRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User FatchUses(User user)
        {
            var result = new User();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM users WHERE Name = @ID ", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", user.UserName);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var _data = new User
                            {
                                UserId = reader.GetInt32(0),
                                UserName = reader.GetString(1),
                                Role = reader.GetString(2),
                                Email = reader.GetString(3),
                                UserCreated = reader.GetDateTime(reader.GetOrdinal("UserCreated")),
                                Password = reader.GetString(5),
                            };
                            result = _data;
                        }
                    }

                }
            }
            return result;
        }
    }
}
