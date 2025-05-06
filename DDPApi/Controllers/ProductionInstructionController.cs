using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DDPApi.Data;
using DDPApi.DTO;
using DDPApi.Interfaces;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProductionInstructionController : ControllerBase
{
    private readonly IProductionInstruction _productionInstructionService;
    private readonly AppDbContext _context;

    public ProductionInstructionController(
        IProductionInstruction productionInstructionService,
        AppDbContext context)
    {
        _productionInstructionService = productionInstructionService;
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProductionInstruction(
        [FromBody] ProductionInstructionDto instructionDto)
    {
        if (instructionDto == null)
            return BadRequest("Üretim talimatı boş olamaz");

        try
        {
            var createdInstruction = await _productionInstructionService
                .CreateProductionInstructionAsync(instructionDto);
            return CreatedAtAction(
                nameof(CreateProductionInstruction),
                new { id = createdInstruction.Id },
                createdInstruction);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("company-instructions")]
    public async Task<IActionResult> GetProductionInstructionsByCompanyId()
    {
        try
        {
            var instructions = await _productionInstructionService
                .GetProductionInstructionsByCompanyIdAsync();
            return Ok(instructions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("enter")]
    public async Task<IActionResult> EnterMachine([FromBody] MachineOperationDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var (valid, message, production) = await _productionInstructionService
                .ValidateProduction(dto.Barcode);
            if (!valid) return BadRequest(message);

            var (checkSuccess, checkMessage) = await _productionInstructionService
                .CheckPreviousMachines(production!, dto.MachineId);
            if (!checkSuccess) return BadRequest(checkMessage);

            var currentMachine = production!.ProductionToMachines
                .First(pm => pm.MachineId == dto.MachineId);

            var (sessionSuccess, sessionMessage, session) = await _productionInstructionService
                .GetOrCreateSession(production, dto.MachineId, dto.Barcode, dto.Count);
            if (!sessionSuccess) return BadRequest(sessionMessage);

            await _productionInstructionService.UpdateMachineStatus(currentMachine, 1);

            session!.status = 1;
            session.count += dto.Count;

            await _productionInstructionService.CheckProductionCompletion(production);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { Success = true, Message = "Makineye giriş başarılı" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("exit")]
    public async Task<IActionResult> ExitMachine([FromBody] MachineOperationDto dto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var (valid, message, production) = await _productionInstructionService
                .ValidateProduction(dto.Barcode);
            if (!valid) return BadRequest(message);

            var session = production!.ProductToSeans
                .FirstOrDefault(s => s.machineId == dto.MachineId && s.barcode == dto.Barcode);

            if (session == null)
                return BadRequest("Bu makinede aktif seans bulunamadı!");

            if (session.status != 1)
                return BadRequest("Bu makinede çıkış yapılabilir durumda seans bulunamadı!");

            var currentMachine = production.ProductionToMachines
                .First(pm => pm.MachineId == dto.MachineId);

            session.status = 2;
            await _productionInstructionService.UpdateMachineStatus(currentMachine, 2);

            await _productionInstructionService.CheckProductionCompletion(production);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { Success = true, Message = "Makinadan çıkış başarılı" });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { Success = false, Message = ex.Message });
        }
    }
}

public class MachineOperationDto
{
    public int MachineId { get; set; }
    public string Barcode { get; set; }
    public int Count { get; set; }
}