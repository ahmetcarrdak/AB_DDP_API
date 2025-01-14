namespace DDPApi.Models;

public class WorkforcePlanning
{
    public int WorkforcePlanningId { get; set; }      // İş gücü planlaması ID'si
    public int EmployeeId { get; set; }               // Çalışan ID'si
    public int StationId { get; set; }                // İstasyon/Makine ID'si
    public DateTime ShiftStart { get; set; }          // Vardiya başlama saati
    public DateTime ShiftEnd { get; set; }            // Vardiya bitiş saati
    public string TaskDescription { get; set; }       // Görev açıklaması
    public bool IsAssigned { get; set; }              // Görev atanıp atanmadığı
}
