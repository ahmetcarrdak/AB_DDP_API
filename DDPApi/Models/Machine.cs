using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DDPApi.Models
{
    public class Machine
    {
        [Key]
        public int MachineId { get; set; } 
        // Makine kaydı için benzersiz bir kimlik numarası.

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        // Makinenin adı (örn. CNC Tezgahı, Kaynak Robotu).

        [Required]
        [MaxLength(50)]
        public string Code { get; set; }
        // Makineye atanmış benzersiz kod (seri numarası veya envanter kodu gibi).

        [Required]
        [MaxLength(100)]
        public string Location { get; set; }
        // Makinenin bulunduğu lokasyon (ör. Fabrika 1, Üretim Hattı A).

        [MaxLength(200)]
        public string Manufacturer { get; set; }
        // Makinenin üreticisi (ör. Siemens, Fanuc).

        [MaxLength(100)]
        public string Model { get; set; }
        // Makinenin modeli (örn. ABC123).

        [Required]
        public DateTime PurchaseDate { get; set; }
        // Makinenin satın alındığı tarih.

        [Required]
        public bool IsOperational { get; set; }
        // Makinenin şu anda çalışıp çalışmadığını belirten alan (true: Çalışıyor, false: Çalışmıyor).

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Kaydın oluşturulma tarihi (varsayılan olarak şu anki UTC zamanı atanır).

        public DateTime? UpdatedAt { get; set; }
        // Kaydın en son güncellenme tarihi (opsiyonel).

        public virtual ICollection<MachineFault> Faults { get; set; }
        // Makinenin geçmişteki arızalarını tutan koleksiyon.
    }
}