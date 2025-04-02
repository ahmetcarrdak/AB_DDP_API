using System;
using System.ComponentModel.DataAnnotations;

namespace DDPApi.DTO
{
    public class MachineCreateDto
    {
        [Required(ErrorMessage = "Makine adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Makine adı 2-100 karakter aralığında olmalıdır")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Model bilgisi zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Model bilgisi 2-100 karakter aralığında olmalıdır")]
        public string Model { get; set; }

        // KRİTİK OPSİYONEL ALANLAR
        [StringLength(100, ErrorMessage = "Barkod maksimum 100 karakter olabilir")]
        public string Barcode { get; set; }

        [StringLength(100, ErrorMessage = "Seri numarası maksimum 100 karakter olabilir")]
        public string SerialNumber { get; set; }

        // TEKNİK ÖZELLİKLER (Tamamen opsiyonel)
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string Manufacturer { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; }

        public int? WarrantyPeriod { get; set; } // in months

        [StringLength(100, ErrorMessage = "Power consumption cannot exceed 100 characters")]
        public string PowerConsumption { get; set; }

        [StringLength(100, ErrorMessage = "Dimensions cannot exceed 100 characters")]
        public string Dimensions { get; set; }

        [StringLength(100, ErrorMessage = "Weight cannot exceed 100 characters")]
        public string Weight { get; set; }
    }
}