using System;
using System.ComponentModel.DataAnnotations;

namespace DDPApi.Models
{
    public class Station
    {
        [Key] public int StationId { get; set; }

        // İstasyonun adı
        public string Name { get; set; }

        // İstasyonun tipi (1: Sipariş İstasyonu, 2: İş İstasyonu, 3: hem iş hem sipariş)
        public int StationType { get; set; }

        // İstasyonun açıklaması
        public string Description { get; set; }

        // İstasyonun aktif/pasif durumu
        public bool IsActive { get; set; }

        // İstasyonun sırası/önceliği
        public int OrderNumber { get; set; }

        // İstasyonda çalışabilecek maksimum kişi sayısı
        public int MaxWorkerCount { get; set; }

        // İstasyonun ortalama işlem süresi (dakika)
        public int AverageProcessTime { get; set; }

        // İstasyonun bulunduğu bölüm/alan
        public string Department { get; set; }

        // İstasyonun sorumlusu
        public int ResponsiblePersonId { get; set; }

        // İstasyonun oluşturulma tarihi
        public DateTime CreatedAt { get; set; }

        // İstasyonu oluşturan kullanıcı ID
        public int CreatedBy { get; set; }

        // İstasyonun son güncelleme tarihi
        public DateTime? UpdatedAt { get; set; }

        // İstasyonu son güncelleyen kullanıcı ID
        public int? UpdatedBy { get; set; }

        // İstasyonun özel gereksinimleri veya notları
        public string SpecialNotes { get; set; }

        // İstasyonun kalite kontrol gerektirip gerektirmediği
        public bool RequiresQualityCheck { get; set; }

        // İstasyonun bakım durumu
        public bool MaintenanceRequired { get; set; }

        // Son bakım tarihi
        public DateTime? LastMaintenanceDate { get; set; }

        // Bir sonraki planlanan bakım tarihi
        public DateTime? NextMaintenanceDate { get; set; }
    }
}