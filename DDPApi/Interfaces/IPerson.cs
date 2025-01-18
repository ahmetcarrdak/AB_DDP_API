using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.DTO;

public interface IPerson
{
    Task<PersonDto> GetPersonByIdAsync(int id);                     // ID'ye göre personel getir
    
    Task<List<PersonDto>> GetAllPersonsAsync();                     // Tüm personelleri getir
    
    Task<List<PersonDto>> GetActivePersonsAsync();                  // Aktif personelleri getir
    
    Task<PersonDto> GetPersonByIdentityNumberAsync(string identityNumber);  // TC Kimlik numarasına göre personel getir
    
    Task<List<PersonDto>> GetPersonsByDepartmentAsync(string department);   // Departmana göre personelleri getir
    
    Task<PersonDto> AddPersonAsync(PersonDto person);                  // Yeni personel ekle
    
    Task<PersonDto> UpdatePersonAsync(PersonUpdateDto person);               // Personel bilgilerini güncelle
    
    Task<bool> DeletePersonAsync(int id);                        // Personeli sil
    
    Task<bool> DeactivatePersonAsync(int id);                    // Personeli pasif duruma getir
    
    Task<List<PersonDto>> GetPersonsByPositionAsync(string position);       // Pozisyona göre personelleri getir
    
    Task<decimal> GetTotalSalaryAsync();                         // Toplam maaş giderini hesapla
    
    Task<int> GetTotalActivePersonCountAsync();                  // Toplam aktif personel sayısını getir
    
    Task<List<PersonDto>> GetPersonsByHireDateRangeAsync(DateTime startDate, DateTime endDate);  // İşe giriş tarih aralığına göre personelleri getir
    
    Task<List<PersonDto>> SearchPersonsAsync(string searchTerm);    // Personellerde arama yap
    
    Task<bool> UpdateSalaryAsync(int id, decimal newSalary);     // Personel maaşını güncelle
    
    Task<bool> UpdateDepartmentAsync(int id, string newDepartment);      // Personel departmanını güncelle
    
    Task<List<PersonDto>> GetPersonsWithExpiredHealthCheckAsync();  // Sağlık kontrolü geçmiş personelleri getir
    
    Task<int> GetRemainingVacationDaysAsync(int id);            // Kalan izin günlerini hesapla
    
    Task<List<PersonDto>> GetPersonsByShiftScheduleAsync(string shiftSchedule);    // Vardiya planına göre personelleri getir
    
    Task<bool> UpdateEmergencyContactAsync(int id, string contact, string phone);    // Acil durum kontakt bilgilerini güncelle
}
