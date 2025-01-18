using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Models;

namespace DDPApi.Interfaces
{
    public interface IStages
    {
        Task<List<Stages>> GetAllStages();
        Task<Stages> GetStageById(int stageId);
        Task<Stages> CreateStage(Stages stage);
        Task<Stages> UpdateStage(Stages stage);
        Task<bool> DeleteStage(int stageId);
    }
}