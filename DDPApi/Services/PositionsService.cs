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
    public class PositionsService : IPositions
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int _companyId;

        public PositionsService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Positions>> GetAllPositions()
        {
            return await _context.Positions
                .ToListAsync();
        }

        public async Task<Positions> GetPositionById(int positionId)
        {
            return await _context.Positions
                .Where(p => p.PositionId == positionId)
                .FirstOrDefaultAsync();
        }

        public async Task<Positions> CreatePosition(Positions position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<Positions> UpdatePosition(Positions position)
        {
            var existingPosition = await _context.Positions
                .Where(p => p.PositionId == position.PositionId)
                .FirstOrDefaultAsync();

            if (existingPosition != null)
            {
                existingPosition.PositionName = position.PositionName;
                existingPosition.PositionDescription = position.PositionDescription;
                await _context.SaveChangesAsync();
            }

            return existingPosition;
        }

        public async Task<bool> DeletePosition(int positionId)
        {
            var position = await _context.Positions
                .Where(p => p.PositionId == positionId)
                .FirstOrDefaultAsync();

            if (position == null)
                return false;

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}