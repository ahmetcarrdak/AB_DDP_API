using System;
using System.ComponentModel.DataAnnotations;
using DDPApi.Models;

namespace DDPApi.DTO
{
    public class OrderDto
    {
        public int OrderId { get; set; } // Benzersiz kimlik numarası

        [Required] public DateTime OrderDate { get; set; } // Sipariş tarihi

        [Required] [StringLength(100)] public string CustomerName { get; set; } // Müşteri adı/firma adı

        [Required] [StringLength(200)] public string DeliveryAddress { get; set; } // Teslimat adresi

        [StringLength(15)] public string? CustomerPhone { get; set; } // Telefon numarası (opsiyonel)

        [EmailAddress] [StringLength(100)] public string? CustomerEmail { get; set; } // E-posta adresi (opsiyonel)

        [Required] [StringLength(50)] public string ProductName { get; set; } // Ürün adı

        [Required] public int Quantity { get; set; } // Miktar

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        public decimal UnitPrice { get; set; } // Birim fiyat

        [StringLength(50)] public string? OrderStatus { get; set; } // Sipariş durumu (opsiyonel)

        public DateTime? EstimatedDeliveryDate { get; set; } // Tahmini teslimat tarihi

        public DateTime? ActualDeliveryDate { get; set; } // Gerçek teslimat tarihi

        [StringLength(50)] public string? PaymentMethod { get; set; } // Ödeme yöntemi (opsiyonel)

        public bool IsPaid { get; set; } = false; // Ödeme durumu, varsayılan olarak `false`

        [StringLength(50)] public string? PaymentStatus { get; set; } // Ödeme durumu detayı (opsiyonel)

        public int? AssignedEmployeeId { get; set; } // Atanan çalışan (opsiyonel)

        [StringLength(200)] public string? SpecialInstructions { get; set; } // Özel talimatlar/notlar (opsiyonel)

        [StringLength(50)] public string? Priority { get; set; } // Öncelik durumu (opsiyonel)

        public bool IsActive { get; set; } = true; // Sipariş aktiflik durumu, varsayılan olarak `true`

        [StringLength(100)] public string? CancellationReason { get; set; } // İptal nedeni (opsiyonel)

        public DateTime? CancellationDate { get; set; } // İptal tarihi (opsiyonel)

        [StringLength(50)] public string? OrderSource { get; set; } // Sipariş kaynağı (opsiyonel)

        public decimal? DiscountAmount { get; set; } // İndirim tutarı (opsiyonel)

        public decimal? TaxAmount { get; set; } // Vergi tutarı (opsiyonel)

        [StringLength(50)] public string? InvoiceNumber { get; set; } // Fatura numarası (opsiyonel)
        
        public ToPersons ToPersons { get; set; }
    }

    public class OrderStationDto
    {
        public int? StationId { get; set; }
        public int StagesId { get; set; }
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public string CustomerName { get; set; }
        public string? SpecialInstructions { get; set; }
        public string? Priority { get; set; }
        public int? AssignedEmployeeId { get; set; }
        public int Quantity { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; } // Tahmini teslimat tarihi
        public DateTime? ActualDeliveryDate { get; set; }
    }
}