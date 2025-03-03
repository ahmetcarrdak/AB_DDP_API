using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Data;
using DDPApi.Interfaces;
using DDPApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DDPApi.Services
{
    public class StagesService : IStages
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public StagesService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Stages>> GetAllStages()
        {
            return await _context.Stages
                .Where(s => s.CompanyId == _companyId)
                .ToListAsync();
        }

        public async Task<Stages> GetStageById(int stageId)
        {
            return await _context.Stages
                .Where(s => s.CompanyId == _companyId && s.StageId == stageId)
                .FirstOrDefaultAsync();
        }

        public async Task<Stages> CreateStage(Stages stage)
        {
            stage.CompanyId = _companyId;
            _context.Stages.Add(stage);
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<Stages> UpdateStage(Stages stage)
        {
            var existingStage = await _context.Stages
                .Where(s => s.CompanyId == _companyId && s.StageId == stage.StageId)
                .FirstOrDefaultAsync();

            if (existingStage != null)
            {
                existingStage.StageName = stage.StageName;
                await _context.SaveChangesAsync();
            }

            return existingStage;
        }

        public async Task<bool> DeleteStage(int stageId)
        {
            var stage = await _context.Stages
                .Where(s => s.CompanyId == _companyId && s.StageId == stageId)
                .FirstOrDefaultAsync();

            if (stage == null)
                return false;

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}