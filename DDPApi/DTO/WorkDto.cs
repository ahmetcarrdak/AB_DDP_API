namespace DDPApi.DTO;

public class WorkDto
{
    public int WorkId { get; set; }                             // İş benzersiz kimlik numarası
    public string WorkName { get; set; }                        // İş/görev adı
    public string Description { get; set; }                     // İş tanımı ve detayları
    public DateTime CreatedDate { get; set; }                   // İşin oluşturulma tarihi
    public DateTime? StartDate { get; set; }                    // İşin başlama tarihi
    public DateTime? DueDate { get; set; }                      // İşin teslim tarihi
    public DateTime? CompletionDate { get; set; }               // İşin tamamlanma tarihi
    public string Status { get; set; }                          // İş durumu
    public string Priority { get; set; }                        // Öncelik seviyesi
    public int? AssignedEmployeeId { get; set; }                // İşi yapacak personel ID'si
    public string? Location { get; set; }                       // İşin yapılacağı lokasyon
    public decimal? EstimatedCost { get; set; }                 // Tahmini maliyet
    public decimal? ActualCost { get; set; }                    // Gerçekleşen maliyet
    public int? EstimatedDuration { get; set; }                 // Tahmini süre (saat)
    public int? ActualDuration { get; set; }                    // Gerçekleşen süre (saat)
    public string? RequiredEquipment { get; set; }              // Gerekli ekipman listesi
    public string? RequiredMaterials { get; set; }              // Gerekli malzeme listesi
    public bool IsRecurring { get; set; }                       // Periyodik iş mi?
    public string? RecurrencePattern { get; set; }              // Tekrarlanma şablonu
    public bool RequiresApproval { get; set; }                  // Onay gerektiriyor mu?
    public string? Notes { get; set; }                          // Ek notlar
    public bool IsActive { get; set; }                          // İş aktif mi?
    public string? CancellationReason { get; set; }             // İptal nedeni
    public DateTime? CancellationDate { get; set; }             // İptal tarihi
    public int? QualityScore { get; set; }                      // Kalite puanı
    public string? QualityNotes { get; set; }                   // Kalite değerlendirme notları
    public bool HasSafetyRisks { get; set; }                    // Güvenlik riski var mı?
    public string? SafetyNotes { get; set; }                    // Güvenlik notları
}

public class WorkStationDto
{
    public int? StationId { get; set; }                    
    public int StagesId { get; set; }
    public int WorkId { get; set; }                             // İş benzersiz kimlik numarası
    public string WorkName { get; set; }                        // İş/görev adı
    public string Description { get; set; }      
}