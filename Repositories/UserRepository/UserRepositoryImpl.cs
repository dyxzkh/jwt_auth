using asp.net_jwt.Data;
using asp.net_jwt.Data.Models;
using asp.net_jwt.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore;

public class UserRepositoryImpl : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepositoryImpl(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckEmailIfExist(string email)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

        if (existingUser != null)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> CheckUsernameIfExist(string username)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        if (existingUser != null)
        {
            return true;
        }

        return false;
    }

    public async Task CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAll()
    {
        var users = await _context.Users.AsNoTracking().ToListAsync();
        return users;
    }

    public async Task<User?> GetById(int id)
    {
        var existingUser = await _context.Users.FindAsync(id);

        if (existingUser == null)
        {
            return null;
        }

        return existingUser;
    }

    public async Task<User?> GetByEmail(string email)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (existingUser == null)
        {
            return null;
        }
        return existingUser;
    }

    public async Task<User?> GetByUsername(string username)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        if (existingUser == null)
        {
            return null;
        }
        return existingUser;
    }

    public async Task UpdateUser(User user)
    {
        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUser(int id)
    {
        var existingUser = await GetById(id);
        if (existingUser != null)
        {
            _context.Remove(existingUser);
            await _context.SaveChangesAsync();
        }
    }
}
