using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Data;

public class PersonService : IPerson
{
    private readonly AppDbContext _context;

    public PersonService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Person> GetPersonByIdAsync(int id)
    {
        return await _context.Persons.FindAsync(id);
    }

    public async Task<List<Person>> GetAllPersonsAsync()
    {
        return await _context.Persons.ToListAsync();
    }

    public async Task<List<Person>> GetActivePersonsAsync()
    {
        return await _context.Persons.Where(p => p.IsActive).ToListAsync();
    }

    public async Task<Person> GetPersonByIdentityNumberAsync(string identityNumber)
    {
        return await _context.Persons.FirstOrDefaultAsync(p => p.IdentityNumber == identityNumber);
    }

    public async Task<List<Person>> GetPersonsByDepartmentAsync(string department)
    {
        return await _context.Persons.Where(p => p.Department == department).ToListAsync();
    }

    public async Task<Person> AddPersonAsync(Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<Person> UpdatePersonAsync(Person person)
    {
        _context.Persons.Update(person);
        await _context.SaveChangesAsync();
        return person;
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

    public async Task<List<Person>> GetPersonsByPositionAsync(string position)
    {
        return await _context.Persons.Where(p => p.Position == position).ToListAsync();
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

    public async Task<List<Person>> GetPersonsByHireDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Persons
            .Where(p => p.HireDate >= startDate && p.HireDate <= endDate)
            .ToListAsync();
    }

    public async Task<List<Person>> SearchPersonsAsync(string searchTerm)
    {
        return await _context.Persons
            .Where(p => p.FirstName.Contains(searchTerm) || 
                       p.LastName.Contains(searchTerm) || 
                       p.Department.Contains(searchTerm) ||
                       p.Position.Contains(searchTerm))
            .ToListAsync();
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

    public async Task<List<Person>> GetPersonsWithExpiredHealthCheckAsync()
    {
        var today = DateTime.Today;
        return await _context.Persons
            .Where(p => p.LastHealthCheck == null || p.LastHealthCheck.Value.AddYears(1) < today)
            .ToListAsync();
    }

    public async Task<int> GetRemainingVacationDaysAsync(int id)
    {
        var person = await _context.Persons.FindAsync(id);
        return person?.VacationDays ?? 0;
    }

    public async Task<List<Person>> GetPersonsByShiftScheduleAsync(string shiftSchedule)
    {
        return await _context.Persons
            .Where(p => p.ShiftSchedule == shiftSchedule)
            .ToListAsync();
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
}
