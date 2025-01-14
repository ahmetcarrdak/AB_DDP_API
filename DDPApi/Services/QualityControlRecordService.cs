using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models; 
using DDPApi.Data; 
using DDPApi.Interfaces;

namespace DDPApi.Services
{
    public class QualityControlRecordService : IQualityControlRecord
    {
        private readonly AppDbContext _context;

        public QualityControlRecordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QualityControlRecord> AddQualityControlRecordAsync(QualityControlRecord qualityControlRecord)
        {
            _context.QualityControlRecords.Add(qualityControlRecord);
            await _context.SaveChangesAsync();
            return qualityControlRecord;
        }

        public async Task<QualityControlRecord> UpdateQualityControlRecordAsync(int id, QualityControlRecord qualityControlRecord)
        {
            var existingRecord = await _context.QualityControlRecords.FindAsync(id);
            if (existingRecord != null)
            {
                existingRecord.ProductId = qualityControlRecord.ProductId;
                existingRecord.TestType = qualityControlRecord.TestType;
                existingRecord.TestResult = qualityControlRecord.TestResult;
                existingRecord.TestDate = qualityControlRecord.TestDate;
                existingRecord.TestedBy = qualityControlRecord.TestedBy;
                existingRecord.Comments = qualityControlRecord.Comments;

                await _context.SaveChangesAsync();
            }
            return existingRecord;
        }

        public async Task<bool> DeleteQualityControlRecordAsync(int id)
        {
            var record = await _context.QualityControlRecords.FindAsync(id);
            if (record != null)
            {
                _context.QualityControlRecords.Remove(record);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<QualityControlRecord> GetQualityControlRecordByIdAsync(int id)
        {
            return await _context.QualityControlRecords.FindAsync(id);
        }

        public async Task<IEnumerable<QualityControlRecord>> GetAllQualityControlRecordsAsync()
        {
            return await _context.QualityControlRecords.ToListAsync();
        }
    }
}
