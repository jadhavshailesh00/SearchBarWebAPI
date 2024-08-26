using Microsoft.AspNetCore.Mvc;

namespace Interview.Entity
{
    public class TokenRequest
    {
        [FromForm(Name = "client_id")]
        public string client_id { get; set; }

        [FromForm(Name = "grant_type")]
        public string GrantType { get; set; }

        [FromForm(Name = "client_secret")]
        public string client_secret { get; set; }

        [FromForm(Name = "username")]
        public string Username { get; set; }

        [FromForm(Name = "password")]
        public string Password { get; set; }

        [FromForm(Name = "scope")]
        public string Scope { get; set; }
    }
}
