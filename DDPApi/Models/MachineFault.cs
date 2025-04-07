using System;
using System.ComponentModel.DataAnnotations;

namespace DDPApi.Models
{
    public class MachineFault
    {
        [Key] public int FaultId { get; set; }
        // Arıza kaydı için benzersiz bir kimlik numarası.

        [Required] public int MachineId { get; set; }
        // Arızaya neden olan makinenin adı.

        [MaxLength(50)] public string MachineCode { get; set; }
        // Makineye ait benzersiz kod (örn. seri numarası veya envanter numarası).

        [Required] public DateTime FaultStartDate { get; set; }
        // Arızanın başlangıç tarihi ve saati.

        public DateTime? FaultEndDate { get; set; }
        // Arızanın bitiş tarihi ve saati. (Opsiyonel: Henüz çözülmeyen arızalar için null olabilir.)

        [Required][MaxLength(200)] public string FaultDescription { get; set; }
        // Arızanın kısa açıklaması.

        [MaxLength(200)] public string Cause { get; set; }
        // Arızanın sebebi (eğer biliniyorsa).

        [MaxLength(200)] public string Solution { get; set; }
        // Arıza çözümü ile ilgili açıklama.

        public string FaultSeverity { get; set; }
        // Arızanın şiddeti (örn. Düşük, Orta, Yüksek, Kritik).

        public string ReportedBy { get; set; }
        // Arızayı bildiren kişi veya departman.

        public string ResolvedBy { get; set; }
        // Arızayı çözen kişi veya ekip. (Opsiyonel)

        [Required] public bool IsResolved { get; set; }
        // Arızanın çözülüp çözülmediğini belirten alan (true: çözülmüş, false: çözülmemiş).

        // Makinenin ne kadar arızalandığını tutar
        public int TotalFault { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        // Arıza kaydının oluşturulma tarihi.

        public DateTime? UpdatedAt { get; set; }
        // Arıza kaydının güncellenme tarihi (opsiyonel).

        public Machine? Machine { get; set; }
    }
}