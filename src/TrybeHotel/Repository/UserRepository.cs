using TrybeHotel.Models;
using TrybeHotel.Dto;
// using TrybeHotel.Services;
using Microsoft.AspNetCore.Mvc;
using System.CodeDom.Compiler;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null) return null!;
            return new UserDto{
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }

        public UserDto Login(LoginDto login)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email);
            if (user == null || user.Password != login.Password) return null!;
            return new UserDto{
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var formatUser = new User{
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };
            var newUser = _context.Users.Add(formatUser).Entity;
            _context.SaveChanges();
            return new UserDto{
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType,
            };
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user == null) return null!;
            return new UserDto{
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            };
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.Select(u => new UserDto{
                Email = u.Email,
                Name = u.Name,
                UserId = u.UserId,
                UserType = u.UserType,
            }).ToList();
            return users;
        }

    }
}