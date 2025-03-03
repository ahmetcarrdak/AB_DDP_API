using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IAuth
{
    Task<(bool success, string message, string token, Person user, Company company)> Login(string identityNumber, string password);
    Task<(bool success, string message, string token, Person user, Company company)> Register(
        string firstName,
        string lastName,
        string identityNumber,
        string password,
        string companyName,
        string companyTaxNumber,
        string companyAddress,
        string companyPhone,
        string companyEmail);
}