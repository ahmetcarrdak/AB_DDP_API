using System.ComponentModel.DataAnnotations;

namespace DDPApi.Models
{
    public class Positions
    {
        [Key]
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionDescription { get; set; }
    }
}