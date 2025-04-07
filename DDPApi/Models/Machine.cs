using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDPApi.Models
{
    public class Machine
    {
        [Key] public int Id { get; set; }

        // Zorunlu Temel Bilgiler
        [Required(ErrorMessage = "Şirket ID zorunludur")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Makine adı zorunludur")]
        [StringLength(100, ErrorMessage = "Makine adı maksimum 100 karakter olabilir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Model bilgisi zorunludur")]
        [StringLength(100, ErrorMessage = "Model bilgisi maksimum 100 karakter olabilir")]
        public string Model { get; set; }

        // Opsiyonel Temel Bilgiler
        [StringLength(100, ErrorMessage = "Barkod maksimum 100 karakter olabilir")]
        public string Barcode { get; set; }

        [StringLength(100, ErrorMessage = "Seri numarası maksimum 100 karakter olabilir")]
        public string SerialNumber { get; set; }

        // Zaman Bilgileri
        public DateTime? PurchaseDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Teknik Özellikler (Opsiyonel)
        [StringLength(100, ErrorMessage = "Üretici bilgisi maksimum 100 karakter olabilir")]
        public string Manufacturer { get; set; }

        [Column(TypeName = "decimal(18,2)")] public decimal? PurchasePrice { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama maksimum 500 karakter olabilir")]
        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "Konum bilgisi maksimum 100 karakter olabilir")]
        public string Location { get; set; }

        public int? WarrantyPeriod { get; set; } // Ay cinsinden

        [StringLength(100, ErrorMessage = "Güç tüketimi bilgisi maksimum 100 karakter olabilir")]
        public string PowerConsumption { get; set; }

        [StringLength(100, ErrorMessage = "Boyut bilgisi maksimum 100 karakter olabilir")]
        public string Dimensions { get; set; }

        [StringLength(100, ErrorMessage = "Ağırlık bilgisi maksimum 100 karakter olabilir")]
        public string Weight { get; set; }

        // Durum Bilgileri
        public bool IsActive { get; set; } = true;
        public bool IsOperational { get; set; } = true;
        public int TotalFault { get; set; } = 0;
        public int IsDeleted { get; set; } = 0;
        public DateTime DeletedDate { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual Company Company { get; set; }
    }
}