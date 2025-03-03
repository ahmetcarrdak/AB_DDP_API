using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DDPApi.Models; 
using DDPApi.Data; 
using DDPApi.Interfaces;
using System.Linq;

namespace DDPApi.Services
{
    public class QualityControlRecordService : IQualityControlRecord
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public QualityControlRecordService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            
            // JWT'den CompanyId'yi al
            var companyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId");
            if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
            {
                _companyId = companyId;
            }
        }

        public async Task<QualityControlRecord> AddQualityControlRecordAsync(QualityControlRecord qualityControlRecord)
        {
            qualityControlRecord.CompanyId = _companyId;
            _context.QualityControlRecords.Add(qualityControlRecord);
            await _context.SaveChangesAsync();
            return qualityControlRecord;
        }

        public async Task<QualityControlRecord> UpdateQualityControlRecordAsync(int id, QualityControlRecord qualityControlRecord)
        {
            var existingRecord = await _context.QualityControlRecords
                .Where(r => r.CompanyId == _companyId && r.QualityControlRecordId == id)
                .FirstOrDefaultAsync();

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
            var record = await _context.QualityControlRecords
                .Where(r => r.CompanyId == _companyId && r.QualityControlRecordId == id)
                .FirstOrDefaultAsync();

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
            return await _context.QualityControlRecords
                .Where(r => r.CompanyId == _companyId && r.QualityControlRecordId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QualityControlRecord>> GetAllQualityControlRecordsAsync()
        {
            return await _context.QualityControlRecords
                .Where(r => r.CompanyId == _companyId)
                .ToListAsync();
        }
    }
}
