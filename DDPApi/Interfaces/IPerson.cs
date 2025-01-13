using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPerson
{
    Task<Person> GetPersonByIdAsync(int id);                     // ID'ye göre personel getir
    
    Task<List<Person>> GetAllPersonsAsync();                     // Tüm personelleri getir
    
    Task<List<Person>> GetActivePersonsAsync();                  // Aktif personelleri getir
    
    Task<Person> GetPersonByIdentityNumberAsync(string identityNumber);  // TC Kimlik numarasına göre personel getir
    
    Task<List<Person>> GetPersonsByDepartmentAsync(string department);   // Departmana göre personelleri getir
    
    Task<Person> AddPersonAsync(Person person);                  // Yeni personel ekle
    
    Task<Person> UpdatePersonAsync(Person person);               // Personel bilgilerini güncelle
    
    Task<bool> DeletePersonAsync(int id);                        // Personeli sil
    
    Task<bool> DeactivatePersonAsync(int id);                    // Personeli pasif duruma getir
    
    Task<List<Person>> GetPersonsByPositionAsync(string position);       // Pozisyona göre personelleri getir
    
    Task<decimal> GetTotalSalaryAsync();                         // Toplam maaş giderini hesapla
    
    Task<int> GetTotalActivePersonCountAsync();                  // Toplam aktif personel sayısını getir
    
    Task<List<Person>> GetPersonsByHireDateRangeAsync(DateTime startDate, DateTime endDate);  // İşe giriş tarih aralığına göre personelleri getir
    
    Task<List<Person>> SearchPersonsAsync(string searchTerm);    // Personellerde arama yap
    
    Task<bool> UpdateSalaryAsync(int id, decimal newSalary);     // Personel maaşını güncelle
    
    Task<bool> UpdateDepartmentAsync(int id, string newDepartment);      // Personel departmanını güncelle
    
    Task<List<Person>> GetPersonsWithExpiredHealthCheckAsync();  // Sağlık kontrolü geçmiş personelleri getir
    
    Task<int> GetRemainingVacationDaysAsync(int id);            // Kalan izin günlerini hesapla
    
    Task<List<Person>> GetPersonsByShiftScheduleAsync(string shiftSchedule);    // Vardiya planına göre personelleri getir
    
    Task<bool> UpdateEmergencyContactAsync(int id, string contact, string phone);    // Acil durum kontakt bilgilerini güncelle
}

