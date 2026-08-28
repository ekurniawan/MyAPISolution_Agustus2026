using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyAPISolution.SampleAPI.DAL
{
    public class AuthDAL : IAuthDAL
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthDAL(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task AddRole(string roleName)
        {
            try
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    throw new Exception("Role already exists");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding role: {ex.Message}");
            }
        }

        public async Task AddRolesToUser(string username, List<string> roleNames)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                foreach (var roleName in roleNames)
                {
                    var roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                    {
                        throw new ArgumentException($"Role '{roleName}' not found");
                    }
                }
                var result = await _userManager.AddToRolesAsync(user, roleNames);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding roles to user: {ex.Message}");
            }
        }

        public async Task AddUsersToRole(string roleName, List<string> users)
        {
            try
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    throw new ArgumentException("Role not found");
                }
                foreach (var username in users)
                {
                    var user = await _userManager.FindByNameAsync(username);
                    if (user == null)
                    {
                        throw new ArgumentException($"User '{username}' not found");
                    }
                }
                foreach (var username in users)
                {
                    var user = await _userManager.FindByNameAsync(username);
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding users to role: {ex.Message}");
            }
        }

        public async Task AddUserToRole(string username, string roleName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    throw new ArgumentException("Role not found");
                }
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding user to role: {ex.Message}");
            }
        }

        public async Task DeleteRole(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new ArgumentException("Role not found");
                }
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting role: {ex.Message}");
            }
        }

        public async Task<IEnumerable<IdentityRole>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<IdentityRole> GetRole(string roleName)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new ArgumentException("Role not found");
                }
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting role: {ex.Message}");
            }
        }

        public async Task<IEnumerable<string>> GetRolesFromUser(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                var roles = await _userManager.GetRolesAsync(user);
                return roles;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting roles from user: {ex.Message}");
            }
        }

        public async Task<IdentityUser> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }
            return user;
        }

        public async Task<IdentityUser> GetUserById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting user by ID: {ex.Message}");
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }
                var result = await _userManager.CheckPasswordAsync(user, password);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error logging in: {ex.Message}");
            }
        }

        public async Task<IdentityUser> Register(IdentityUser user, string password)
        {
            try
            {
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    return user;
                }
                else
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

            }
            catch (Exception ex)
            {

                throw new Exception($"Error registering user: {ex.Message}");
            }
        }
    }
}
