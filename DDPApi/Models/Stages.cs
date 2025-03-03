using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DDPApi.Models
{
    public class Stages
    {
        [Key]
        public int StageId { get; set; }
        public int CompanyId { get; set; }
        public string StageName { get; set; }
    }
}