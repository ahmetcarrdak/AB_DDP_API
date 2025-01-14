namespace DDPApi.Models;

public class Alert
{
    public int AlertId { get; set; }                  // Uyarı ID'si
    public int MachineId { get; set; }                // İlgili makine ID'si
    public string AlertType { get; set; }             // Uyarı tipi (Arıza, Bakım, Enerji)
    public string Message { get; set; }               // Uyarı mesajı
    public DateTime AlertDate { get; set; }           // Uyarı tarihi
    public bool IsResolved { get; set; }              // Uyarının çözülüp çözülmediği
    public string ResolvedBy { get; set; }            // Çözüme kavuşturan kişi (varsa)
}
