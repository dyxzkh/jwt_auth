using asp.net_jwt.Data.Models;
using asp.net_jwt.DTOs;

namespace asp.net_jwt.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAll();

        Task<User?> GetById(int id);

        Task<User?> GetByUsername(string username);

        Task<User?> GetByEmail(string email);

        Task CreateUser(User user);

        Task UpdateUser(User user);

        Task DeleteUser(int id);

        Task<bool> CheckUsernameIfExist(string username);

        Task<bool> CheckEmailIfExist(string email);
    }
}
