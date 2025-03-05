using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Data;
using DDPApi.Models;
using DDPApi.DTO;
using System.Linq;

public class PersonService : IPerson
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PersonService> _logger;
    private readonly int _companyId;

    public PersonService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<PersonService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        
        // JWT'den CompanyId'yi al
        var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId");
        if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
        {
            _companyId = companyId;
        }
    }

    public async Task<PersonDto> GetPersonByIdAsync(int id)
    {
        var person = await _context.Persons
            .Include(p => p.Position)
            .FirstOrDefaultAsync(p => p.CompanyId == _companyId && p.Id == id);
        return MapToDto(person);
    }
    
    public async Task<bool> PersonGetInSession()
    {
        var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

        if (int.TryParse(userIdString, out var userId))
        {
            var person = await _context.Persons
                .Include(p => p.Position)
                .FirstOrDefaultAsync(p => p.CompanyId == _companyId && p.Id == userId);

            return person != null;
        }

        return false;
    }


    public async Task<List<PersonDto>> GetAllPersonsAsync()
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.CompanyId == _companyId)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<PersonDto> AddPersonAsync(PersonDto personDto)
    {
        var person = MapToPerson(personDto);
        person.CompanyId = _companyId;
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
        return MapToDto(person);
    }

    public async Task<PersonDto> UpdatePersonAsync(PersonUpdateDto personDto)
    {
        // Güncellenecek personeli veritabanından al
        var person = await _context.Persons
            .Where(p => p.CompanyId == _companyId && p.Id == personDto.Id)
            .FirstOrDefaultAsync();

        if (person == null)
        {
            throw new ArgumentException("Person not found");
        }

        // DTO'daki değişiklikleri mevcut person nesnesine uygula
        MapToPersonForUpdate(person, personDto);

        // Veritabanında değişiklikleri güncelle
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();

        // Güncellenmiş person nesnesini DTO'ya dönüştür ve geri döndür
        return MapToDto(person);
    }

    public async Task<bool> DeletePersonAsync(int id)
    {
        var person = await _context.Persons
            .Where(p => p.CompanyId == _companyId && p.Id == id)
            .FirstOrDefaultAsync();
            
        if (person == null) return false;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CollectivePersonUpdateAsync(List<PersonCollectiveUpdateDto> personUpdates)
    {
        if (personUpdates.Count == 0) return false;

        // Tüm personel ID'lerini al
        var personIds = personUpdates.Select(p => p.Id).ToList();

        // Veritabanından ilgili personelleri çek
        var persons = await _context.Persons
            .Where(p => p.CompanyId == _companyId && personIds.Contains(p.Id))
            .ToListAsync();
            
        if (persons.Count == 0) return false;

        // Her bir personel için güncelleme yap
        foreach (var personUpdate in personUpdates)
        {
            var person = persons.FirstOrDefault(p => p.Id == personUpdate.Id);
            if (person != null)
            {
                person.FirstName = personUpdate.FirstName;
                person.LastName = personUpdate.LastName;
                person.Department = personUpdate.Department;
                person.PositionId = personUpdate.PositionId;
                person.PhoneNumber = personUpdate.PhoneNumber;
                person.Email = personUpdate.Email;
                person.IsActive = personUpdate.IsActive;
            }
        }

        // Değişiklikleri kaydet
        _context.Persons.UpdateRange(persons);
        await _context.SaveChangesAsync();
        return true;
    }

    private DateTime ConvertToUtcDateTime(DateTime? excelRowBirthDate)
    {
        throw new NotImplementedException();
    }

    private PersonDto MapToDto(Person person)
    {
        if (person == null) return null;

        return new PersonDto
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            IdentityNumber = person.IdentityNumber,
            BirthDate = person.BirthDate,
            Address = person.Address,
            PhoneNumber = person.PhoneNumber,
            Email = person.Email,
            HireDate = person.HireDate,
            TerminationDate = person.TerminationDate,
            Department = person.Department,
            PositionId = person.PositionId,
            Salary = person.Salary,
            IsActive = person.IsActive,
            BloodType = person.BloodType,
            EmergencyContact = person.EmergencyContact,
            EmergencyPhone = person.EmergencyPhone,
            EducationLevel = person.EducationLevel,
            HasDriverLicense = person.HasDriverLicense,
            Notes = person.Notes,
            VacationDays = person.VacationDays,
            HasHealthInsurance = person.HasHealthInsurance,
            LastHealthCheck = person.LastHealthCheck,
            ShiftSchedule = person.ShiftSchedule,
        };
    }

    private void MapToPersonForUpdate(Person person, PersonUpdateDto dto)
    {
        person.Id = dto.Id;
        person.PhoneNumber = dto.PhoneNumber;
        person.Email = dto.Email;
        person.TerminationDate = dto.TerminationDate;
        person.Department = dto.Department;
        person.PositionId = dto.PositionId;
        person.Salary = dto.Salary;
        person.Address = dto.Address;
        person.EmergencyContact = dto.EmergencyContact;
        person.EmergencyPhone = dto.EmergencyPhone;
        person.EducationLevel = dto.EducationLevel;
        person.IsActive = dto.IsActive;
        person.HasDriverLicense = dto.HasDriverLicense;
        person.HasHealthInsurance = dto.HasHealthInsurance;
        person.LastHealthCheck = dto.LastHealthCheck;
        person.ShiftSchedule = dto.ShiftSchedule;
        person.VacationDays = dto.VacationDays;
        person.Notes = dto.Notes;
    }

    private Person MapToPerson(PersonDto dto)
    {
        if (dto == null) return null;

        return new Person
        {
            Id = dto.Id,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            IdentityNumber = dto.IdentityNumber,
            BirthDate = dto.BirthDate,
            Address = dto.Address,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email,
            HireDate = dto.HireDate,
            TerminationDate = dto.TerminationDate,
            Department = dto.Department,
            PositionId = dto.PositionId,
            Salary = dto.Salary,
            IsActive = dto.IsActive,
            BloodType = dto.BloodType,
            EmergencyContact = dto.EmergencyContact,
            EmergencyPhone = dto.EmergencyPhone,
            EducationLevel = dto.EducationLevel,
            HasDriverLicense = dto.HasDriverLicense,
            Notes = dto.Notes,
            VacationDays = dto.VacationDays,
            HasHealthInsurance = dto.HasHealthInsurance,
            LastHealthCheck = dto.LastHealthCheck,
            ShiftSchedule = dto.ShiftSchedule
        };
    }
}