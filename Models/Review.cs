using System;
using System.ComponentModel.DataAnnotations;

namespace JanHofmeyerAdmin.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Your Name")]
        public string ReviewerName { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Service Used")]
        public string ServiceUsed { get; set; } // This will be populated from Projects list

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        [Display(Name = "Your Review")]
        public string ReviewMessage { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;

        public bool IsDisplayed { get; set; } = false;

        // Optional: Track the reviewer type
        [MaxLength(50)]
        public string ReviewerType { get; set; } // e.g., "Beneficiary", "Volunteer", "Partner"

        // Optional: For verified reviews
        public bool IsVerified { get; set; } = false;
    }
}