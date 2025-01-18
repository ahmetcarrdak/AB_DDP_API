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

        public PositionsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Positions>> GetAllPositions()
        {
            return await _context.Positions.ToListAsync();
        }

        public async Task<Positions> GetPositionById(int positionId)
        {
            return await _context.Positions.FindAsync(positionId);
        }

        public async Task<Positions> CreatePosition(Positions position)
        {
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<Positions> UpdatePosition(Positions position)
        {
            _context.Entry(position).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return position;
        }

        public async Task<bool> DeletePosition(int positionId)
        {
            var position = await _context.Positions.FindAsync(positionId);
            if (position == null)
                return false;

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}