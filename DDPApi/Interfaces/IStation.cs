using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces
{
    public interface IStation
    {
        Task<List<Station>> GetAllStationsAsync();  // Tüm istasyonları listeler

        Task<Station> GetStationByIdAsync(int id);  // ID'ye göre istasyon getirir

        Task<Station> CreateStationAsync(Station station);  // Yeni istasyon oluşturur

        Task<Station> UpdateStationAsync(int id, Station station);  // Mevcut istasyonu günceller

        Task<bool> DeleteStationAsync(int id);  // İstasyonu siler

        Task<List<Station>> GetActiveStationsAsync();  // Aktif istasyonları listeler

        Task<List<Station>> GetStationsByTypeAsync(int stationType);  // Tipe göre istasyonları listeler

        Task<List<Station>> GetStationsByDepartmentAsync(string department);  // Bölüme göre istasyonları listeler

        Task<bool> UpdateMaintenanceStatusAsync(int id, bool maintenanceRequired);  // Bakım durumunu günceller

        Task<List<Station>> GetStationsRequiringMaintenanceAsync();  // Bakım gerektiren istasyonları listeler

        Task<bool> UpdateQualityCheckStatusAsync(int id, bool requiresQualityCheck);  // Kalite kontrol durumunu günceller

        Task<List<Station>> GetStationsByResponsiblePersonAsync(int responsiblePersonId);  // Sorumlu kişiye göre istasyonları listeler

        Task<IEnumerable<object>> GetTopStationsWithMostPendingJobsAndStagesAsync(); // Yığılmaları gösterir
    }
}