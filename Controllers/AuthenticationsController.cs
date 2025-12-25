using asp.net_jwt.Data.Response;
using asp.net_jwt.DTOs;
using asp.net_jwt.Services.AuthenticationService;
using Microsoft.AspNetCore.Mvc;

namespace asp.net_jwt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationsController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] Login req)
        {
            try
            {
                var response = await _authenticationService.Login(req);

                if (response != null)
                {
                    return Ok(response);
                }

                return BadRequest(new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Incorrect username or password!",
                    Details = ""
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred.",
                    Details = ex.ToString()
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenDto dto)
        {
            try
            {
                var response = await _authenticationService.RefreshToken(dto.Username, dto.RefreshToken);

                if (response != null)
                {
                    return Ok(response);
                }

                return BadRequest(new ApiResponse
                {
                    StatusCode = 500,
                    Message = "Incorrect username or password!",
                    Details = ""
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new ApiResponse
                {
                    StatusCode = 500,
                    Message = ex.Message,
                    Details = ex.ToString()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred.",
                    Details = ex.ToString()
                });
            }
        }
    }
}
