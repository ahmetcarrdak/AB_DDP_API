using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Data;
using DDPApi.Models;
using DDPApi.DTO;

public class PersonService : IPerson
{
    private readonly AppDbContext _context;

    public PersonService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PersonDto> GetPersonByIdAsync(int id)
    {
        var person = await _context.Persons
            .Include(p => p.Position)
            .FirstOrDefaultAsync(p => p.Id == id);
        return MapToDto(person);
    }

    public async Task<List<PersonDto>> GetAllPersonsAsync()
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<List<PersonDto>> GetActivePersonsAsync()
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.IsActive)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<PersonDto> GetPersonByIdentityNumberAsync(string identityNumber)
    {
        var person = await _context.Persons
            .Include(p => p.Position)
            .FirstOrDefaultAsync(p => p.IdentityNumber == identityNumber);
        return MapToDto(person);
    }

    public async Task<List<PersonDto>> GetPersonsByDepartmentAsync(string department)
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.Department == department)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<PersonDto> AddPersonAsync(PersonDto personDto)
    {
        var person = MapToPerson(personDto);
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
        return MapToDto(person);
    }

    public async Task<PersonDto> UpdatePersonAsync(PersonUpdateDto personDto)
    {
        // Güncellenecek personeli veritabanından al
        var person = await _context.Persons.FindAsync(personDto.Id);

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
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivatePersonAsync(int id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        person.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PersonDto>> GetPersonsByPositionAsync(string position)
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.Position.PositionName == position)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<decimal> GetTotalSalaryAsync()
    {
        return await _context.Persons
            .Where(p => p.IsActive)
            .SumAsync(p => p.Salary.GetValueOrDefault());
    }

    public async Task<int> GetTotalActivePersonCountAsync()
    {
        return await _context.Persons.CountAsync(p => p.IsActive);
    }

    public async Task<List<PersonDto>> GetPersonsByHireDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.HireDate >= startDate && p.HireDate <= endDate)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<List<PersonDto>> SearchPersonsAsync(string searchTerm)
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.FirstName.Contains(searchTerm) ||
                       p.LastName.Contains(searchTerm) ||
                       p.Department.Contains(searchTerm) ||
                       p.Position.PositionName.Contains(searchTerm))
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<bool> UpdateSalaryAsync(int id, decimal newSalary)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        person.Salary = newSalary;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateDepartmentAsync(int id, string newDepartment)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        person.Department = newDepartment;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<PersonDto>> GetPersonsWithExpiredHealthCheckAsync()
    {
        var today = DateTime.Today;
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.LastHealthCheck == null || p.LastHealthCheck.Value.AddYears(1) < today)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<int> GetRemainingVacationDaysAsync(int id)
    {
        var person = await _context.Persons.FindAsync(id);
        return person?.VacationDays ?? 0;
    }

    public async Task<List<PersonDto>> GetPersonsByShiftScheduleAsync(string shiftSchedule)
    {
        var persons = await _context.Persons
            .Include(p => p.Position)
            .Where(p => p.ShiftSchedule == shiftSchedule)
            .ToListAsync();
        return persons.Select(MapToDto).ToList();
    }

    public async Task<bool> UpdateEmergencyContactAsync(int id, string contact, string phone)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        person.EmergencyContact = contact;
        person.EmergencyPhone = phone;
        await _context.SaveChangesAsync();
        return true;
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
            PositionName = person.Position?.PositionName
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
