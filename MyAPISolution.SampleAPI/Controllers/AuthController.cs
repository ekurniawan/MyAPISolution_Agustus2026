using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyAPISolution.SampleAPI.DAL;
using MyAPISolution.SampleAPI.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyAPISolution.SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthDAL _authDAL;
        public AuthController(IAuthDAL authDAL)
        {
            _authDAL = authDAL;
        }

        //AddUserToRole
        [HttpPost("UserRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] UserRoleDTO userRoleDTO)
        {
            try
            {
                await _authDAL.AddUserToRole(userRoleDTO.Username, userRoleDTO.RoleName);
                return Ok($"User '{userRoleDTO.Username}' added to role '{userRoleDTO.RoleName}' successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UsersRole")]
        public async Task<IActionResult> AddUsersToRole([FromBody] UsersRoleDTO usersRoleDTO)
        {
            try
            {
                await _authDAL.AddUsersToRole(usersRoleDTO.RoleName, usersRoleDTO.Usernames);
                return Ok($"Users added to role '{usersRoleDTO.RoleName}' successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
        {
            try
            {
                var user = new IdentityUser
                {
                    UserName = userRegisterDTO.Username,
                    Email = userRegisterDTO.Email
                };

                var result = await _authDAL.Register(user, userRegisterDTO.Password);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //add role
        [HttpPost("Role")]
        public async Task<IActionResult> AddRole([FromBody] string roleName)
        {
            try
            {
                await _authDAL.AddRole(roleName);
                return Ok($"Role '{roleName}' added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //get user
        [HttpGet("User")]
        public async Task<IActionResult> GetUser(string username)
        {
            try
            {
                var user = await _authDAL.GetUser(username);
                if (user == null)
                {
                    return NotFound($"User '{username}' not found.");
                }
                var userDTO = new UserDTO
                {
                    Username = user.UserName,
                    Email = user.Email
                };
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //get all role
        [HttpGet("Roles")]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _authDAL.GetAllRoles();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //get all users
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authDAL.GetAllUsers();
                List<UserDTO> userDTOs = new List<UserDTO>();
                foreach(var user in users)
                {
                    userDTOs.Add(new UserDTO
                    {
                        Username = user.UserName,
                        Email = user.Email
                    });
                }
                return Ok(userDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
