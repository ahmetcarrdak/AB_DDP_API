using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IAuth
{
    Task<(bool success, string message, string token, Person user, Company company)> PersonLogin(string identityNumber, string password);
    Task<(bool success, string message, string token, Company company)> CompanyLogin(string taxNumber, string password);
    Task<(bool success, string message, string token ,Company company)> Register(
        string password,
        string companyName,
        string companyTaxNumber,
        string companyAddress,
        string companyPhone,
        string companyEmail);
}