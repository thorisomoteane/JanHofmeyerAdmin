using System;
using System.ComponentModel.DataAnnotations;

namespace JanHofmeyerAdmin.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string ShortSummary { get; set; } = "";  // Add default

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(100)]
        public string Theme { get; set; }

        public int? PeopleImpacted { get; set; }

        public string AboutSection { get; set; } = "";  // Add default empty string

        public string KeyGoals { get; set; } = "";  // Add default

        [MaxLength(500)]
        public string ImageUrl { get; set; } = "";  // Add default

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}