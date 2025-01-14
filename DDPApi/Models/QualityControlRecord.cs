namespace DDPApi.Models;

public class QualityControlRecord
{
    public int QualityControlRecordId { get; set; }   // Kalite kontrol kaydı ID'si
    public int ProductId { get; set; }                 // Ürün ID'si
    public string TestType { get; set; }               // Test tipi (Ölçü, Ağırlık, vb.)
    public string TestResult { get; set; }             // Test sonucu (Geçti, Kaldı)
    public DateTime TestDate { get; set; }             // Test tarihi
    public string TestedBy { get; set; }               // Testi yapan kişi
    public string Comments { get; set; }               // Test ile ilgili yorumlar
}
