using DDPApi.DTO;
using DDPApi.Models;
using System.Threading.Tasks;

namespace DDPApi.Interfaces;

public interface IProductionInstruction
{
    Task<ProductionInstruction> CreateProductionInstructionAsync(ProductionInstructionDto instructionDto);
    Task<List<ProductionInstruction>> GetProductionInstructionsByCompanyIdAsync();
}