using asp.net_jwt.Data.Response;
using asp.net_jwt.DTOs;

namespace asp.net_jwt.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<JWTResponse?> Login(Login login);

        Task<JWTResponse?> RefreshToken(string username, string refreshToken);
    }
}
