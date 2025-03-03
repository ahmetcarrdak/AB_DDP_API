using System;
using System.Threading.Tasks;
using DDPApi.Models;
using DDPApi.Data;
using Microsoft.EntityFrameworkCore;
using DDPApi.Interfaces;

namespace DDPApi.Services
{
    public class AuthService : IAuth
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<(bool success, string message, string token, Person user, Company company)> Register(
            string firstName,
            string lastName,
            string identityNumber,
            string password,
            string companyName,
            string companyTaxNumber,
            string companyAddress,
            string companyPhone,
            string companyEmail)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if user already exists
                var existingUser = await _context.Persons
                    .FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber);

                if (existingUser != null)
                    return (false, "Bu TC Kimlik numarası ile kayıtlı kullanıcı bulunmaktadır.", null, null, null);

                // Check if company exists with same tax number
                var existingCompany = await _context.Companies
                    .FirstOrDefaultAsync(c => c.TaxNumber == companyTaxNumber);

                if (existingCompany != null)
                    return (false, "Bu vergi numarası ile kayıtlı firma bulunmaktadır.", null, null, null);

                // Create new company
                var company = new Company
                {
                    Name = companyName,
                    TaxNumber = companyTaxNumber,
                    Address = companyAddress,
                    PhoneNumber = companyPhone,
                    Email = companyEmail,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    CompanyId = Guid.NewGuid()
                };

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();

                // Create new user
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                var user = new Person
                {
                    FirstName = firstName,
                    LastName = lastName,
                    IdentityNumber = identityNumber,
                    PasswordHash = passwordHash,
                    CompanyId = company.CompanyId,
                    Company = company,
                    Role = "Admin", // İlk kayıt olan kişi Admin olur
                    IsActive = true,
                    HireDate = DateTime.Now
                };

                _context.Persons.Add(user);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                // Generate token
                var token = _jwtService.GenerateToken(user);

                return (true, "Firma ve kullanıcı başarıyla oluşturuldu.", token, user, company);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Kayıt işlemi sırasında bir hata oluştu: {ex.Message}", null, null, null);
            }
        }

        public async Task<(bool success, string message, string token, Person user, Company company)> Login(string identityNumber, string password)
        {
            var user = await _context.Persons
               .Include(c => c.Company)
               .FirstOrDefaultAsync(u => u.IdentityNumber == identityNumber);

            if (user == null)
                return (false, "Kullanıcı bulunamadı.", null, null, null);

            if (!user.IsActive)
                return (false, "Kullanıcı hesabı aktif değil.", null, null, null);

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (false, "Şifre hatalı.", null, null, null);

            var token = _jwtService.GenerateToken(user);

            return (true, "Giriş başarılı.", token, user, user.Company);
        }
    }
}