using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDPApi.Models;

public class Work
{
    [Key]
    public int WorkId { get; set; }                             // İş benzersiz kimlik numarası

    [Required]
    public int CompanyId { get; set; }
    // Şirketin benzersiz kimlik numarası.

    public int StationId { get; set; } = 1;                    // İstasyon ID'si
    public int StagesId { get; set; } = 1;                       // istasyonda ki aşamayı belirtir

    public string? Barcode {get; set;} 

    [Required]
    [StringLength(100)]
    public string WorkName { get; set; }                        // İş/görev adı

    [Required]
    [StringLength(500)]
    public string? Description { get; set; }                     // İş tanımı ve detayları

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow; // İşin oluşturulma tarihi (varsayılan: şu anki zaman)

    public DateTime? StartDate { get; set; }                    // İşin başlama tarihi

    public DateTime? DueDate { get; set; }                      // İşin teslim tarihi

    public DateTime? CompletionDate { get; set; }               // İşin tamamlanma tarihi

    [Required]
    [StringLength(50)]
    public int? Status { get; set; }                          // İş durumu

    [Required]
    [StringLength(50)]
    public string? Priority { get; set; }                        // Öncelik seviyesi
    public int? AssignedEmployeeId { get; set; }                // İşi yapacak personel ID'si (opsiyonel)

    public int? CreatedByEmployeeId { get; set; }                // İşi oluşturan personel ID'si

    [StringLength(100)]
    public string? Location { get; set; }                       // İşin yapılacağı lokasyon (opsiyonel)

    [Column(TypeName = "decimal(18,2)")]
    public decimal? EstimatedCost { get; set; }                 // Tahmini maliyet

    [Column(TypeName = "decimal(18,2)")]
    public decimal? ActualCost { get; set; }                    // Gerçekleşen maliyet

    public int? EstimatedDuration { get; set; }                 // Tahmini süre (saat)

    public int? ActualDuration { get; set; }                    // Gerçekleşen süre (saat)

    [StringLength(100)]
    public string? RequiredEquipment { get; set; }              // Gerekli ekipman listesi (opsiyonel)

    [StringLength(100)]
    public string? RequiredMaterials { get; set; }              // Gerekli malzeme listesi (opsiyonel)

    [StringLength(50)]
    public string? WorkType { get; set; }                       // İş tipi (opsiyonel)

    public bool? IsRecurring { get; set; } = false;              // Periyodik iş mi? (varsayılan: hayır)

    [StringLength(50)]
    public string? RecurrencePattern { get; set; }              // Tekrarlanma şablonu (opsiyonel)

    public bool? RequiresApproval { get; set; } = false;         // Onay gerektiriyor mu? (varsayılan: hayır)

    public int? ApprovedByEmployeeId { get; set; }              // Onaylayan personel ID'si (opsiyonel)

    public DateTime? ApprovalDate { get; set; }                 // Onay tarihi (opsiyonel)

    [StringLength(500)]
    public string? Notes { get; set; }                          // Ek notlar (opsiyonel)

    public bool? IsActive { get; set; } = true;                  // İş aktif mi? (varsayılan: evet)

    [StringLength(100)]
    public string? CancellationReason { get; set; }             // İptal nedeni (opsiyonel)

    public DateTime? CancellationDate { get; set; }             // İptal tarihi (opsiyonel)

    public int? QualityScore { get; set; }                      // Kalite puanı (opsiyonel)

    [StringLength(500)]
    public string? QualityNotes { get; set; }                   // Kalite değerlendirme notları (opsiyonel)

    public bool? HasSafetyRisks { get; set; } = false;           // Güvenlik riski var mı? (varsayılan: hayır)

    [StringLength(500)]
    public string? SafetyNotes { get; set; }                    // Güvenlik notları ve önlemleri (opsiyonel)
    public Station Station { get; set; }
    public Stages Stages { get; set; }
}
