using System;
using System.Threading.Tasks;
using DDPApi.Models;
using DDPApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class TokenResponse
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public int CompanyId { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var (success, message, user) = await _authService.AuthenticateAsync(request.Username, request.Password);

            if (!success)
                return Unauthorized(new { message });

            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            await _authService.UpdateRefreshTokenAsync(user, refreshToken);

            return Ok(new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = user.Username,
                Role = user.Role,
                CompanyId = user.CompanyId
            });
        }

        public class RefreshTokenRequest
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Invalid token request");

            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = int.Parse(principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var isValid = await _authService.ValidateRefreshTokenAsync(userId, request.RefreshToken);
            if (!isValid)
                return BadRequest("Invalid refresh token");

            var user = await _authService.AuthenticateAsync(principal.Identity.Name, "");
            if (!user.success)
                return BadRequest("User not found");

            var newAccessToken = _jwtService.GenerateToken(user.user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            await _authService.UpdateRefreshTokenAsync(user.user, newRefreshToken);

            return Ok(new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Username = user.user.Username,
                Role = user.user.Role,
                CompanyId = user.user.CompanyId
            });
        }
    }
} 