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

        public StagesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stages>> GetAllStages()
        {
            return await _context.Stages.ToListAsync();
        }

        public async Task<Stages> GetStageById(int stageId)
        {
            return await _context.Stages.FindAsync(stageId);
        }

        public async Task<Stages> CreateStage(Stages stage)
        {
            _context.Stages.Add(stage);
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<Stages> UpdateStage(Stages stage)
        {
            _context.Entry(stage).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return stage;
        }

        public async Task<bool> DeleteStage(int stageId)
        {
            var stage = await _context.Stages.FindAsync(stageId);
            if (stage == null)
                return false;

            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}