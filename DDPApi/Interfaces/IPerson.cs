using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.DTO;
using Microsoft.AspNetCore.Mvc;

public interface IPerson
{
    Task<PersonDto> GetPersonByIdAsync(int id);                     // ID'ye göre personel getir
    
    Task<List<PersonDto>> GetAllPersonsAsync();                     // Tüm personelleri getir
    
    Task<PersonDto> AddPersonAsync(PersonDto person);                  // Yeni personel ekle
    
    Task<PersonDto> UpdatePersonAsync(PersonUpdateDto person);               // Personel bilgilerini güncelle
    
    Task<bool> DeletePersonAsync(int id);                        // Personeli sil
    
    Task<bool> CollectivePersonUpdateAsync(List<PersonCollectiveUpdateDto> personUpdates);        // Toplu işlem yaparak personel bilgilerini güncelle
}
