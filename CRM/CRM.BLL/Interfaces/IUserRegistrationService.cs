using CRM.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Interfaces
{
    public interface IUserRegistrationService
    {
        Task CreateUser(CreateUserDTO CreateUserDTO);
        Task<GetUserDTO> GetUser(string UserId);
        Task<IEnumerable<GetUserDTO>> GetUsers();
        Task DeleteUser(string UserId);
        Task<GetUserDTO> GetCurrent(string currentUserName);
        Task<GetUserDTO> UpdateUser(GetUserDTO UserDTO);
        Task<IEnumerable<string>> GetUserRoles(string UserId);
        Task<IEnumerable<GetUserDTO>> GetRoleUsers(string RoleName);
        Task<string> GetUserFullName(string userId);
    }
}
