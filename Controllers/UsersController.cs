using asp.net_jwt.Data;
using asp.net_jwt.Data.Response;
using asp.net_jwt.DTOs;
using asp.net_jwt.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_jwt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger; // Injecting ILogger
        }

        // GET: api/users
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
        }


        // GET: api/users
        [Authorize]
        [HttpGet("test")]
        public async Task<ActionResult<string> GetAllUsersTest()
        {
            try
            {
                return "OK";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = $"User with ID {id} not found.",
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
        }

        // GET: api/users/username/{username}
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            try
            {
                var user = await _userService.GetUserByUsername(username);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"User with username {username} not found.");
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with username {username}.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
        }

        // GET: api/users/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"User with email {email} not found.");
                return NotFound(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with email {email}.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] UserCreateDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            try
            {
                await _userService.CreateUser(user);
                return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Conflict(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            try
            {
                await _userService.UpdateUser(id, user);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Conflict(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"User with ID {id} not found for update.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}.");
                return BadRequest(new ApiResponse { StatusCode = 400, Message = ex.Message, Details = ex.ToString() });
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning($"User with ID {id} not found for deletion.");
                return BadRequest(new ApiResponse
                {
                    StatusCode = 400,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}.");
                return BadRequest(new ApiResponse { StatusCode = 400, Message = ex.Message, Details = ex.ToString() });
            }
        }
    }
}
