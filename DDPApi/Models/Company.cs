using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDPApi.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        public Guid CompanyId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(11)]
        public string TaxNumber { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;
        public string PasswordHash { get; set; }
        
        // Navigation property
        public virtual ICollection<Person> Employees { get; set; }
    }
} 