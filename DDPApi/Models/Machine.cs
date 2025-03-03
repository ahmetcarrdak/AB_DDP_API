using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using DDPApi.Models;

namespace DDPApi.Models
{
    public class Machine
    {
        [Key]
        public int Id { get; set; }
        // Makine kaydı için benzersiz bir kimlik numarası.

        [Required]
        public int CompanyId { get; set; }
        // Şirketin benzersiz kimlik numarası.

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        // Makinenin adı (örn. CNC Tezgahı, Kaynak Robotu).

        [Required]
        [StringLength(100)]
        public string Model { get; set; }
        // Makinenin modeli (örn. ABC123).

        [Required]
        [StringLength(100)]
        public string SerialNumber { get; set; }
        // Makineye atanmış benzersiz kod (seri numarası veya envanter kodu gibi).

        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; }
        // Makinenin üreticisi (ör. Siemens, Fanuc).

        public DateTime PurchaseDate { get; set; }
        // Makinenin satın alındığı tarih.

        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchasePrice { get; set; }

        public DateTime? LastMaintenanceDate { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string Location { get; set; }
        // Makinenin bulunduğu lokasyon (ör. Fabrika 1, Üretim Hattı A).

        public int? WarrantyPeriod { get; set; }

        [StringLength(100)]
        public string PowerConsumption { get; set; }

        [StringLength(100)]
        public string Dimensions { get; set; }

        [StringLength(100)]
        public string Weight { get; set; }

        // Navigation property
        public virtual Company Company { get; set; }

        // Makinenin ne kadar arızalandığını tutar
        public int TotalFault { get; set; } = 0;

        [Required]
        public bool IsOperational { get; set; }
        // Makinenin şu anda çalışıp çalışmadığını belirten alan (true: Çalışıyor, false: Çalışmıyor).

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Kaydın oluşturulma tarihi (varsayılan olarak şu anki UTC zamanı atanır).

        public DateTime? UpdatedAt { get; set; }
        // Kaydın en son güncellenme tarihi (opsiyonel).
    }
}