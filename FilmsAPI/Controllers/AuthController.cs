using FilmsAPI.DTO;
using FilmsAPI.Models;
using FilmsAPI.Services.AuthService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FilmsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var loginResponse = await _authService.LoginAsync(model);

            if (loginResponse == null)
            {
                return Unauthorized("Invalid login credentials.");
            }

            // Trả về cả Token và thông tin Nhân viên
            return Ok(new
            {
                Token = loginResponse.Token,
                User = loginResponse.User,
                Message = "Đăng nhập thành công!"
            });
        }
    }
}
