using System.ComponentModel.DataAnnotations;

namespace DDPApi.DTO;

public class MachineUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    public int CompanyId { get; set; }

    [Required]
    public string Name { get; set; }

    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public bool IsActive { get; set; }
    public string Location { get; set; }
    public string Manufacturer { get; set; }
}
