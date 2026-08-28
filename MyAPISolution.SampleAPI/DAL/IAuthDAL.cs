using Microsoft.AspNetCore.Identity;

namespace MyAPISolution.SampleAPI.DAL
{
    public interface IAuthDAL
    {
        Task<IdentityUser> Register(IdentityUser user, string password);
        Task<bool> Login(string username, string password);
        Task<IdentityUser> GetUser(string username);
        Task<IdentityUser> GetUserById(string id);
        Task<IdentityRole> GetRole(string roleName);
        Task<IEnumerable<IdentityRole>> GetAllRoles();
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<IEnumerable<string>> GetRolesFromUser(string username);
        Task AddRole(string roleName);
        Task AddUserToRole(string username, string roleName);
        Task AddRolesToUser(string username, List<string> roleNames);
        Task AddUsersToRole(string roleName, List<string> users);
        Task DeleteRole(string roleName);
    }
}
