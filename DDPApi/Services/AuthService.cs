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

        public async Task<(bool success, string message, string token ,Company company)> Register(
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
                // Check if company exists with same tax number
                var existingCompany = await _context.Companies
                    .FirstOrDefaultAsync(c => c.TaxNumber == companyTaxNumber);

                if (existingCompany != null)
                    return (false, "Bu vergi numarası ile kayıtlı firma bulunmaktadır.", null, null);

                // Create new company
                var company = new Company
                {
                    Name = companyName,
                    TaxNumber = companyTaxNumber,
                    Address = companyAddress,
                    PhoneNumber = companyPhone,
                    Email = companyEmail,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CompanyId = Guid.NewGuid(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                };

                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var token = _jwtService.GenerateCompanyToken(company);
                return (true, "Firma başarıyla oluşturuldu.", token,company);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Kayıt işlemi sırasında bir hata oluştu: {ex.Message}", null,null);
            }
        }

        public async Task<(bool success, string message, string token, Person user, Company company)> PersonLogin(string identityNumber, string password)
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

            var token = _jwtService.GeneratePersonToken(user);

            return (true, "Giriş başarılı.", token, user, user.Company);
        }

        public async Task<(bool success, string message, string token, Company company)> CompanyLogin(string taxNumber, string password)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.TaxNumber == taxNumber);

            if (company == null)
                return (false, "Firma bulunamadı.", null, null);

            if (!company.IsActive)
                return (false, "Firma hesabı aktif değil.", null, null);

            if (!BCrypt.Net.BCrypt.Verify(password, company.PasswordHash))
                return (false, "Şifre hatalı.", null, null);

            var token = _jwtService.GenerateCompanyToken(company);

            return (true, "Giriş başarılı.", token, company);
        }
    }
}