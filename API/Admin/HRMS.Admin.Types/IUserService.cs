using HRMS.Admin.Entities;
using HRMS.Admin.Infrastructure.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS.Admin.Types
{
    public interface IUserService
    {
        //Get Users abstract method
        Task<User> GetByUserId(int userId);

        //Get users abstract method
        Task<List<User>> GetAllUsers(bool? isActive = true);

        //Get Users abstract method
        Task<IEnumerable<UserDetails>> GetUsers();
        Task<User> GetById(int userId);
        //Gets user by email
        Task<User> GetUserByEmail(string email);
        // Get active user by userId
        Task<User> GetActiveUserByUserId(int userId);

        //update users
        Task<bool> UpdateUser(int userId);

        //Get Users by roles
        Task<List<UserRoleDetails>> GetUsersByRoles(string roles);

        //Get Users by search string
        Task<List<UserDetails>> GetUsersBySearchString(string searchString);
        string GetUserEmail();
    }
}
