using asp.net_jwt.Data.Models;
using asp.net_jwt.Data.Response;
using asp.net_jwt.DTOs;
using asp.net_jwt.Services.JWT;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using BC = BCrypt.Net.BCrypt;

namespace asp.net_jwt.Services.AuthenticationService
{
    public class AuthenticationServiceImpl : IAuthenticationService
    {

        private readonly UserRepositoryImpl _userRepository;
        private readonly ILogger<AuthenticationServiceImpl> _logger;
        private readonly JWTService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthenticationServiceImpl(UserRepositoryImpl userRepository, 
            ILogger<AuthenticationServiceImpl> logger, 
            JWTService jwtService, 
            IConfiguration configuration,
            IMapper mapper
            ) 
        {
             _userRepository = userRepository;
             _logger = logger;
             _jwtService = jwtService;
             _configuration = configuration;
             _mapper = mapper;
        }

        private JWTResponse GenerateJWT(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpiryDate = DateTime.Now.AddHours(int.Parse(jwtSettings["RefreshTokenExpiresInHours"] ?? "2"));

            //generate jwt
            return new JWTResponse
            {
                Token = _jwtService.GenerateToken($"{user.Username}"),
                RefreshToken = refreshToken,
                ExpiryDate = refreshTokenExpiryDate,
            };
        }

        private async Task UpdateUserToken(User user, string refreshToken, DateTime refreshTokenExpiryDate)
        {
            user.RefreshTokenExpires = refreshTokenExpiryDate;
            user.RefreshToken = refreshToken;
            await _userRepository.UpdateUser(user);
        }

        public async Task<JWTResponse?> Login(Login login)
        {
            var user = await _userRepository.GetByUsername(login.Username);

            if (user == null)
            {
                _logger.LogError($"username with the value of {login.Username} is not found!");
                throw new KeyNotFoundException($"username with the value of {login.Username} is not found!");
            }

            if(BC.Verify(login.Password, user.Password))
            {
                var response = GenerateJWT(user);
                await UpdateUserToken(user, response.RefreshToken, response.ExpiryDate);
                return response;
            }
            return null;
        }


        public async Task<JWTResponse?> RefreshToken(string username, string refreshToken)
        {
            var user = await _userRepository.GetByUsername(username);

            if (user == null)
            {
                _logger.LogError($"username with the value of {username} is not found!");
                throw new KeyNotFoundException($"username with the value of {username} is not found!");
            }

            if (refreshToken == user.RefreshToken && DateTime.Parse(user.RefreshTokenExpires.ToString()) > DateTime.Now)
            {
                var response = GenerateJWT(user);
                await UpdateUserToken(user, response.RefreshToken, response.ExpiryDate);
                return response;
            }
            else
            {
                _logger.LogError($"invalid refresh token or refresh token is expired!");
                throw new InvalidOperationException($"invalid refresh token or refresh token is expired!");
            }
        }
    }
}
