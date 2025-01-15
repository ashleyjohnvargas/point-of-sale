using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using POS1.Controllers;
using System.ComponentModel.DataAnnotations;

// User model for demonstration purposes
namespace POS1.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required, MinLength(14)] //should be 14 and with combination
        [StringLength(255)]
        public string? Password { get; set; } // Store hashed passwords
        public bool IsActive { get; set; }
    }
}

    /*public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; } // Store hashed passwords
    public required string Role { get; set; }
    public required string Status { get; set; }
    public required bool IsActive { get; set; } */

