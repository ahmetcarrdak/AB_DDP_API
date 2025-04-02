using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DDPApi.DTO;
using DDPApi.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class ProductionInstructionController : ControllerBase
{
    private readonly IProductionInstruction _productionInstructionService;

    public ProductionInstructionController(IProductionInstruction productionInstructionService)
    {
        _productionInstructionService = productionInstructionService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateProductionInstruction([FromBody] ProductionInstructionDto instructionDto)
    {
        if (instructionDto == null) return BadRequest("Ürün talimatı boş olamaz.");

        var createdInstruction = await _productionInstructionService.CreateProductionInstructionAsync(instructionDto);
        return CreatedAtAction(nameof(CreateProductionInstruction), new { id = createdInstruction.Id }, createdInstruction);
    }
    
    [HttpGet("company-instructions")]
    public async Task<IActionResult> GetProductionInstructionsByCompanyId()
    {
        try
        {
            var instructions = await _productionInstructionService.GetProductionInstructionsByCompanyIdAsync();
            return Ok(instructions);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPost("process")]
    public async Task<IActionResult> ProcessMachine([FromQuery] int machineId, [FromQuery] string barcode, int count)
    {
        var result = await _productionInstructionService.ProcessMachineOperation(machineId, barcode, count);

        if (result.Contains("hata") || result.Contains("bulunamadı") || result.Contains("tamamlanmadan"))
            return BadRequest(result);

        return Ok(result);
    }
}