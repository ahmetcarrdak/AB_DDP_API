using System.ComponentModel.DataAnnotations;

namespace DDPApi.Models;

public class MaintenanceRecord
{
    public int MaintenanceRecordId { get; set; }  // Bakım kaydının benzersiz ID'si
    
    [Required]
    public int CompanyId { get; set; }
    // Şirketin benzersiz kimlik numarası.

    [Required]
    public int MachineId { get; set; }            // İlgili makine ID'si

    [Required]
    public string MaintenanceType { get; set; }   // Bakım türü (Planlı, Acil, Periyodik)

    [Required]
    public string Description { get; set; }       // Bakım açıklaması
    public DateTime MaintenanceDate { get; set; } // Bakım tarihi
    public string PerformedBy { get; set; }       // Bakımı yapan kişi id
    public string Notes { get; set; }             // Ek notlar (varsa)
}