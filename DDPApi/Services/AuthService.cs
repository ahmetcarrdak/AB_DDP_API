using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DDPApi.Models;
using DDPApi.Data;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public interface IAuthService
    {
        Task<(bool success, string message, Person user)> AuthenticateAsync(string username, string password);
        Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
        Task UpdateRefreshTokenAsync(Person user, string refreshToken);
        string HashPassword(string password, string salt);
        string GenerateSalt();
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool success, string message, Person user)> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Persons
                .Include(p => p.Company)
                .FirstOrDefaultAsync(x => x.Username == username);

            if (user == null)
                return (false, "User not found", null);

            if (!user.IsActive)
                return (false, "User account is deactivated", null);

            var hashedPassword = HashPassword(password, user.Salt);
            if (hashedPassword != user.PasswordHash)
                return (false, "Invalid password", null);

            return (true, "Authentication successful", user);
        }

        public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _context.Persons.FindAsync(userId);
            if (user == null)
                return false;

            return user.RefreshToken == refreshToken && user.RefreshTokenExpiryTime > DateTime.Now;
        }

        public async Task UpdateRefreshTokenAsync(Person user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _context.SaveChangesAsync();
        }

        public string HashPassword(string password, string salt)
        {
            using var sha256 = SHA256.Create();
            var saltedPassword = password + salt;
            var bytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public string GenerateSalt()
        {
            var randomBytes = new byte[32];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
} 