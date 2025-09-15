using System;
using System.ComponentModel.DataAnnotations;

namespace JanHofmeyerAdmin.Models
{
    public class VolunteerApplication
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [MaxLength(50)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Age Group")]
        public string AgeGroup { get; set; } // e.g., "18-25", "26-35", "36-45", "46-55", "56-65", "65+"

        [Required]
        [MaxLength(200)]
        public string Availability { get; set; } // e.g., "Weekdays", "Weekends", "Evenings", "Flexible"

        [Required]
        [Display(Name = "Areas of Interest")]
        public string AreasOfInterest { get; set; } // Multiple areas separated by commas

        [Display(Name = "Relevant Experience")]
        public string RelevantExperience { get; set; }

        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected", "On Hold"

        // Optional: Track who reviewed the application
        public DateTime? ReviewedDate { get; set; }

        [MaxLength(500)]
        public string AdminNotes { get; set; }
    }
}