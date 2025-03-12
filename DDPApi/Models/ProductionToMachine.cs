namespace DDPApi.Models;

public class ProductionToMachine
{
    public int Id { get; set; }
    public int ProductionInstructionId { get; set; }
    public int MachineId { get; set; }
    public int Line { get; set; }
    public int Status { get; set; } = 0; 
    public DateTime? EntryDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExitDate { get; set; } = DateTime.UtcNow;

    public Machine Machine { get; set; }
}