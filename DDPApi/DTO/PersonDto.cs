using System;
using System.ComponentModel.DataAnnotations;

namespace DDPApi.DTO
{
    public class PersonDto
    {
        public int Id { get; set; }

        [Required] [StringLength(50)] public string FirstName { get; set; }

        [Required] [StringLength(50)] public string LastName { get; set; }

        [Required] [StringLength(11)] public string IdentityNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(200)] public string? Address { get; set; }

        [StringLength(15)] public string? PhoneNumber { get; set; }

        [EmailAddress] [StringLength(100)] public string? Email { get; set; }

        [Required] public DateTime HireDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        [StringLength(50)] public string? Department { get; set; }

        [Required] public int PositionId { get; set; }

        public decimal? Salary { get; set; }

        public bool IsActive { get; set; }

        [StringLength(20)] public string? BloodType { get; set; }

        [StringLength(100)] public string? EmergencyContact { get; set; }

        [StringLength(15)] public string? EmergencyPhone { get; set; }

        [StringLength(50)] public string? EducationLevel { get; set; }

        public bool HasDriverLicense { get; set; }

        [StringLength(500)] public string? Notes { get; set; }

        public int VacationDays { get; set; }

        public bool HasHealthInsurance { get; set; }

        public DateTime? LastHealthCheck { get; set; }

        [StringLength(50)] public string? ShiftSchedule { get; set; }
    }

    public class PersonUpdateDto
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Department { get; set; }
        public int PositionId { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyPhone { get; set; }
        public string EducationLevel { get; set; }
        public bool IsActive { get; set; }
        public bool HasDriverLicense { get; set; }
        public bool HasHealthInsurance { get; set; }
        public DateTime? LastHealthCheck { get; set; }
        public string ShiftSchedule { get; set; }
        public int VacationDays { get; set; }
        public string Notes { get; set; }
    }
    public class PersonCollectiveUpdateDto
    {
        public int Id { get; set; } // Güncellenecek personelin ID'si
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public int PositionId { get; set; } // Pozisyon ID'si
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
    
    public class PersonExcelImportDto
    {
        public string FirstName { get; set; }                        // İsim
        public string LastName { get; set; }                         // Soyisim
        public string IdentityNumber { get; set; }                   // TC Kimlik No
        public DateTime? BirthDate { get; set; }                     // Doğum Tarihi
        public string Address { get; set; }                          // Açık Adres
        public string PhoneNumber { get; set; }                      // Telefon Numarası
        public string Email { get; set; }                            // E-Mail Adresi
        public DateTime HireDate { get; set; }                       // İşe Giriş Tarihi
        public string Department { get; set; }                       // Departman
        public int PositionId { get; set; }                          // Pozisyon Numarası
        public decimal Salary { get; set; }                          // Maaş
        public bool IsActive { get; set; }                           // Aktif Çalışıyor mu (Excel'den "evet" veya "hayır" olarak gelir)
        public string BloodType { get; set; }                        // Kan Grubu
        public string EmergencyContact { get; set; }                 // Acil Durum Kişisi
        public string EmergencyPhone { get; set; }                   // Acil Durum Telefon Numarası
        public string EducationLevel { get; set; }                   // Eğitim Seviyesi
        public string DriverLicenseType { get; set; }                // Ehliyet Türü (Excel'den alınır, "B" gibi)
        public string Notes { get; set; }                            // Özel Notlar
        public int VacationDays { get; set; }                        // Yıllık İzin Günü Sayısı
        public bool HasHealthInsurance { get; set; }                 // Sağlık Sigortası Var mı (Excel'den "evet" veya "hayır" olarak gelir)
        public DateTime? LastHealthCheck { get; set; }               // Son Sağlık Kontrolü Tarihi
    }
}