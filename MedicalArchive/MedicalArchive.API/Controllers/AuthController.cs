using MedicalArchive.API.Application.DTOs;
using MedicalArchive.API.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicalArchive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login/user")]
        public async Task<ActionResult<AuthResponseDto>> LoginUser([FromBody] AuthDto authDto)
        {
            var response = await _authService.AuthenticateUserAsync(authDto);
            if (response == null)
            {
                return Unauthorized("Невірний email або пароль");
            }

            return Ok(response);
        }

        [HttpPost("login/doctor")]
        public async Task<ActionResult<AuthResponseDto>> LoginDoctor([FromBody] AuthDto authDto)
        {
            var response = await _authService.AuthenticateDoctorAsync(authDto);
            if (response == null)
            {
                return Unauthorized("Невірний email або пароль");
            }

            return Ok(response);
        }
    }
}
