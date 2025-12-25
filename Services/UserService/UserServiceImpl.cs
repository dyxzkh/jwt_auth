using asp.net_jwt.Data.Models;
using asp.net_jwt.DTOs;
using asp.net_jwt.Repositories.UserRepository;
using asp.net_jwt.Services.JWT;
using AutoMapper;
using BC = BCrypt.Net.BCrypt;

namespace asp.net_jwt.Services.UserService
{
    public class UserServiceImpl : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserServiceImpl> _logger;
        private readonly IMapper _mapper;

        public UserServiceImpl(IUserRepository userRepository, ILogger<UserServiceImpl> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger; // Injecting ILogger
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            var result = _mapper.Map<List<UserDto>>(users);
            return result;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            if(user == null)
            {
                _logger.LogError($"user with the id of {id} cannot be found!");
                throw new KeyNotFoundException($"user with the id of {id} cannot be found!");
            }
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserDto> GetUserByUsername(string username)
        {
            var user = await _userRepository.GetByUsername(username);
            if (user == null)
            {
                _logger.LogError($"user with the username of {username} cannot be found!");
                throw new KeyNotFoundException($"user with the username of {username} cannot be found!");
            }
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetByEmail(email);
            if (user == null)
            {
                _logger.LogError($"user with the email of {email} cannot be found!");
                throw new KeyNotFoundException($"user with the email of {email} cannot be found!");
            }
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task CreateUser(UserCreateDto userDto)
        {
            if (await _userRepository.CheckEmailIfExist(userDto.Email))
            {
                _logger.LogError($"user with the username of {userDto.Username} already exist!");
                throw new InvalidOperationException($"user with the username of {userDto.Username} already exist!");
            }

            if (await _userRepository.CheckUsernameIfExist(userDto.Username))
            {
                _logger.LogError($"user with the username of {userDto.Username} already exist!");
                throw new InvalidOperationException($"user with the username of {userDto.Username} already exist!");
            }

            userDto.Password = BC.HashPassword(userDto.Password);
            var user = _mapper.Map<User>(userDto);
            await _userRepository.CreateUser(user);
        }

        public async Task UpdateUser(int id, UserUpdateDto userDto)
        {
            if (await _userRepository.CheckEmailIfExist(userDto.Email))
            {
                _logger.LogError($"user with the username of {userDto.Username} already exist!");
                throw new InvalidOperationException($"user with the username of {userDto.Username} already exist!");
            }

            if (await _userRepository.CheckUsernameIfExist(userDto.Username))
            {
                _logger.LogError($"user with the username of {userDto.Username} already exist!");
                throw new InvalidOperationException($"user with the username of {userDto.Username} already exist!");
            }

            var user = _mapper.Map<User>(await GetUserById(id));
            await _userRepository.UpdateUser(user);
        }

        public async Task DeleteUser(int id)
        {
           await GetUserById(id);
           await _userRepository.DeleteUser(id);
        }

    }
}
