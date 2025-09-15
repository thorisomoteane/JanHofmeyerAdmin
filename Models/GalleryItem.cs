using System;
using System.ComponentModel.DataAnnotations;

namespace JanHofmeyerAdmin.Models
{
    public class GalleryItem
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(500)]
        public string MediaUrl { get; set; }

        [Required]
        [MaxLength(50)]
        public string MediaType { get; set; } // "Image" or "Video"

        [MaxLength(100)]
        public string Category { get; set; } // e.g., "Volunteering", "Education", "Senior Care", "Food Security", "Health", "Team", "Events", "Infrastructure"

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}