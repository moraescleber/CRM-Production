using AutoMapper;
using CRM.BLL.DTO;
using CRM.BLL.Interfaces;
using CRM.DAL.EF;
using CRM.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Services
{
    public class UserRegistrationService : IUserRegistrationService
    {
        readonly ApiContext dbContext;
        readonly UserManager<User> _userManager;
        public UserRegistrationService(ApiContext context, UserManager<User> userManager)
        {
            dbContext = context;
            _userManager = userManager;
        }
        public async Task CreateUser(CreateUserDTO CreateUserDTO)
        {
            User user = new User
            {
                Email = CreateUserDTO.Email,
                FirstName = CreateUserDTO.FirstName,
                LastName = CreateUserDTO.LastName,
                UserName = CreateUserDTO.Email,

            };

            var result = await _userManager.CreateAsync(user, CreateUserDTO.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, CreateUserDTO.Role);
            }
            else
            {
                string errors = "";
                foreach (var r in result.Errors)
                {
                    errors = errors + r.Description + "\n";
                }
                throw new Exception(errors);
            }
        }

        public async Task DeleteUser(string UserId)
        {
            User user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new Exception("Не правильный id");
            }
            await _userManager.DeleteAsync(user);
        }

        public async Task<GetUserDTO> GetCurrent(string currentUserName)
        {
            User user = await _userManager.FindByNameAsync(currentUserName);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, GetUserDTO>()).CreateMapper();
            GetUserDTO DTO = mapper.Map<User, GetUserDTO>(user);
            return DTO;
        }

        public async Task<IEnumerable<GetUserDTO>> GetRoleUsers(string RoleName)
        {
            IEnumerable<User> Users = await _userManager.GetUsersInRoleAsync(RoleName);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, GetUserDTO>()).CreateMapper();
            IEnumerable<GetUserDTO> UsersDTO = mapper.Map<IEnumerable<GetUserDTO>>(Users);
            return UsersDTO;
        }

        public async Task<GetUserDTO> GetUser(string UserId)
        {
            User model = await _userManager.FindByIdAsync(UserId);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<User, GetUserDTO>());
            var mapper = new Mapper(config);
            GetUserDTO DTO = mapper.Map<GetUserDTO>(model);
            return DTO;
        }

        public async Task<string> GetUserFullName(string userId)
        {
            if (userId == null)
            {
                return "Lead не назначен";
            }
            GetUserDTO user = await GetUser(userId);
            return user.FirstName + " " + user.LastName;
        }

        public async Task<IEnumerable<string>> GetUserRoles(string UserId)
        {
            User user = await _userManager.FindByIdAsync(UserId);
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IEnumerable<GetUserDTO>> GetUsers()
        {
            IEnumerable<User> users = await dbContext.Users.ToListAsync();
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, GetUserDTO>()).CreateMapper();
            IEnumerable<GetUserDTO> DTO = mapper.Map<IEnumerable<GetUserDTO>>(users);
            return DTO;
        }

        public async Task<GetUserDTO> UpdateUser(GetUserDTO UserDTO)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, GetUserDTO>()).CreateMapper();
            User model = mapper.Map<GetUserDTO, User>(UserDTO);
            await _userManager.UpdateAsync(model);
            return UserDTO;
        }
    }
}
