using DDPApi.Models;

namespace DDPApi.Interfaces;

public interface IQualityControlRecord
{
    Task<QualityControlRecord> AddQualityControlRecordAsync(QualityControlRecord qualityControlRecord); // Yeni kalite kontrol kaydı ekleme
    Task<QualityControlRecord> UpdateQualityControlRecordAsync(int id, QualityControlRecord qualityControlRecord); // Kalite kontrol kaydını güncelleme
    Task<bool> DeleteQualityControlRecordAsync(int id); // Kalite kontrol kaydını silme
    Task<QualityControlRecord> GetQualityControlRecordByIdAsync(int id); // Kalite kontrol kaydını ID ile sorgulama
    Task<IEnumerable<QualityControlRecord>> GetAllQualityControlRecordsAsync(); // Tüm kalite kontrol kayıtlarını listeleme

}