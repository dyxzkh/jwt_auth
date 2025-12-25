using asp.net_jwt.DTOs;

namespace asp.net_jwt.Data.Response
{
    public class JWTResponse
    {
        public required string Token {get; set;}

        public required string RefreshToken { get; set; }

        public required DateTime ExpiryDate { get; set; }
    }
}
