using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDPApi.Models;
namespace DDPApi.Interfaces
{
    public interface IPositions
    {
        Task<List<Positions>> GetAllPositions();
        Task<Positions> GetPositionById(int positionId);
        Task<Positions> CreatePosition(Positions position);
        Task<Positions> UpdatePosition(Positions position);
        Task<bool> DeletePosition(int positionId);
    }
}