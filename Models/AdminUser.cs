using System;
using System.ComponentModel.DataAnnotations;

namespace JanHofmeyerAdmin.Models
{
    public class AdminUser
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Optional: Additional admin properties
        [MaxLength(200)]
        public string FullName { get; set; }

        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Role { get; set; } = "Admin"; // Could be "Admin", "SuperAdmin", "Moderator"

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}