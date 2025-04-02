using DDPApi.Models;

namespace DDPApi.DTO;

public class ProductionInstructionDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int CompanyId { get; set; }
    public string Barcode { get; set; }
    public int Count { get; set; }

    public List<ProductionToMachineDto> ProductionToMachines { get; set; } = new List<ProductionToMachineDto>();
    public List<ProductionStore> ProductionStores { get; set; } = new List<ProductionStore>();
}

public class ProductionToMachineDto
{
    public int MachineId { get; set; }
    public int Line { get; set; }
}