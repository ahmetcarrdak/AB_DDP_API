namespace DDPApi.Models;

public class ProductionInstruction
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime InsertDate { get; set; } = DateTime.UtcNow;
    public DateTime? ComplatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedDate { get; set; } = DateTime.UtcNow;
    public int isComplated { get; set; } = 0;
    public int isDeleted { get; set; } = 0;
    public int? MachineId { get; set; } = 0;

    public List<ProductionToMachine> ProductionToMachines { get; set; } = new List<ProductionToMachine>();
    public List<ProductionStore> ProductionStores { get; set; } = new List<ProductionStore>();
}