using System;
using System.Threading.Tasks;
using DDPApi.Models;
using DDPApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DDPApi.Interfaces;

namespace DDPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuth authService, IJwtService jwtService)
        {
            _authService = authService;
            _jwtService = jwtService;
        }

        public class RegisterRequest
        {
            // Kullanıcı Bilgileri
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string IdentityNumber { get; set; }
            public string Password { get; set; }
            
            // Firma Bilgileri
            public string CompanyName { get; set; }
            public string CompanyTaxNumber { get; set; }
            public string CompanyAddress { get; set; }
            public string CompanyPhone { get; set; }
            public string CompanyEmail { get; set; }
        }

        public class LoginRequest
        {
            public string IdentityNumber { get; set; }
            public string Password { get; set; }
        }

        public class AuthResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public string Token { get; set; }
            public Person User { get; set; }
            public Company Company { get; set; }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var (success, message, token, user, company) = await _authService.Register(
                request.FirstName,
                request.LastName,
                request.IdentityNumber,
                request.Password,
                request.CompanyName,
                request.CompanyTaxNumber,
                request.CompanyAddress,
                request.CompanyPhone,
                request.CompanyEmail);

            if (!success)
                return BadRequest(new AuthResponse { Success = false, Message = message });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = message,
                Token = token,
                User = user,
                Company = company
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var (success, message, token, user, company) = await _authService.Login(request.IdentityNumber, request.Password);

            if (!success)
                return Unauthorized(new AuthResponse { Success = false, Message = message });

            return Ok(new AuthResponse
            {
                Success = true,
                Message = "Giriş başarılı",
                Token = token,
                User = user,
                Company = company
            });
        }
    }
} 