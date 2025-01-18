using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDPApi.Models;

public class Person
{
    [Key]
    public int Id { get; set; }                                   // Benzersiz kimlik numarası

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }                        // Personelin adı

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }                         // Personelin soyadı

    [Required]
    [StringLength(11)]
    public string IdentityNumber { get; set; }                   // TC Kimlik numarası (benzersiz olmalı)

    public DateTime? BirthDate { get; set; }                     // Doğum tarihi (opsiyonel)

    [StringLength(200)]
    public string? Address { get; set; }                         // İkamet adresi (opsiyonel)

    [StringLength(15)]
    public string? PhoneNumber { get; set; }                     // Telefon numarası (opsiyonel)

    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }                           // E-posta adresi (opsiyonel)

    [Required]
    public DateTime HireDate { get; set; }                       // İşe giriş tarihi

    public DateTime? TerminationDate { get; set; }               // İşten çıkış tarihi (opsiyonel)

    [StringLength(50)]
    public string? Department { get; set; }                      // Çalıştığı departman (opsiyonel)

    [Required]
    public int PositionId { get; set; }                        // Görevi/Pozisyonu (opsiyonel)

    [Column(TypeName = "decimal(18,2)")]
    public decimal? Salary { get; set; }                         // Maaş bilgisi (opsiyonel)

    public bool IsActive { get; set; } = true;                   // Aktif çalışan durumu (varsayılan: aktif)

    [StringLength(20)]
    public string? BloodType { get; set; }                       // Kan grubu (opsiyonel)

    [StringLength(100)]
    public string? EmergencyContact { get; set; }                // Acil durumda ulaşılacak kişi (opsiyonel)

    [StringLength(15)]
    public string? EmergencyPhone { get; set; }                  // Acil durumda ulaşılacak telefon (opsiyonel)

    [StringLength(50)]
    public string? EducationLevel { get; set; }                  // Eğitim seviyesi (opsiyonel)

    public bool HasDriverLicense { get; set; } = false;          // Sürücü belgesi durumu (varsayılan: yok)

    [StringLength(500)]
    public string? Notes { get; set; }                           // Personel hakkında notlar (opsiyonel)

    public int VacationDays { get; set; } = 0;                   // Yıllık izin günü sayısı (varsayılan: 0)

    public bool HasHealthInsurance { get; set; } = false;        // Sağlık sigortası durumu (varsayılan: yok)

    public DateTime? LastHealthCheck { get; set; }               // Son sağlık kontrolü tarihi (opsiyonel)

    [StringLength(50)]
    public string? ShiftSchedule { get; set; }                   // Vardiya planı (opsiyonel)
    public virtual Positions Position { get; set; }
}
