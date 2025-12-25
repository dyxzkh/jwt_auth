using asp.net_jwt.DTOs;

namespace asp.net_jwt.Services.UserService
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsers();
        Task<UserDto> GetUserById(int id);
        Task<UserDto> GetUserByUsername(string username);
        Task<UserDto> GetUserByEmail(string email);
        Task CreateUser(UserCreateDto user);
        Task UpdateUser(int id, UserUpdateDto user);
        Task DeleteUser(int id);
    }
}
