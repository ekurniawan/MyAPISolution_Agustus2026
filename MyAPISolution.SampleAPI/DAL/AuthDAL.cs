using Microsoft.AspNetCore.Identity;

namespace MyAPISolution.SampleAPI.DAL
{
    public class AuthDAL : IAuthDAL
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public AuthDAL(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            //_roleManager = roleManager;
        }

        public Task AddRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task AddRolesToUser(string username, List<string> roleNames)
        {
            throw new NotImplementedException();
        }

        public Task AddUserToRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityRole> GetRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetRolesFromUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> GetUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityUser> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Login(string username, string password)
        {
            throw new NotImplementedException();
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
