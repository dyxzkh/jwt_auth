using asp.net_jwt.Data.Models;
using asp.net_jwt.DTOs;
using AutoMapper;

namespace asp.net_jwt.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map from User to UserDto
            CreateMap<User, UserDto>();

            // Map from UserCreateDto to User
            CreateMap<UserCreateDto, User>();

            // Map from UserUpdateDto to User
            CreateMap<UserUpdateDto, User>();
        }
    }

}