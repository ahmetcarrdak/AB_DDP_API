using DDPApi.DTO;
using DDPApi.Models;
using System.Threading.Tasks;

namespace DDPApi.Interfaces;

public interface IProductionInstruction
{
    Task<ProductionInstruction> CreateProductionInstructionAsync(ProductionInstructionDto instructionDto);
    Task<List<ProductionInstruction>> GetProductionInstructionsByCompanyIdAsync();
    Task CheckProductionCompletion(ProductionInstruction production);
    Task UpdateMachineStatus(ProductionToMachine machine, int newStatus);

    Task<(bool Success, string Message, ProductToSeans? Session)>
        GetOrCreateSession(ProductionInstruction production, int machineId, string barcode, int count);

    Task<(bool Success, string Message)>
        CheckPreviousMachines(ProductionInstruction production, int currentMachineId);

    Task<(bool Success, string Message, ProductionInstruction? Production)>
        ValidateProduction(string barcode);
}