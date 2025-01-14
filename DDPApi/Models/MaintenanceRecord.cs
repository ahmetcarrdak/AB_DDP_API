namespace DDPApi.Models;

public class MaintenanceRecord
{
    public int MaintenanceRecordId { get; set; }  // Bakım kaydının benzersiz ID'si
    public int MachineId { get; set; }            // İlgili makine ID'si
    public string MaintenanceType { get; set; }   // Bakım türü (Planlı, Acil, Periyodik)
    public string Description { get; set; }       // Bakım açıklaması
    public DateTime MaintenanceDate { get; set; } // Bakım tarihi
    public string PerformedBy { get; set; }       // Bakımı yapan kişi
    public string Notes { get; set; }             // Ek notlar (varsa)
}