namespace asp.net_jwt.DTOs
{
    public class RefreshTokenDto
    {
        public required string Username { get; set; }

        public required string RefreshToken { get; set; }
    }
}
