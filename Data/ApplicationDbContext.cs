using JanHofmeyerAdmin.Models;

using Microsoft.EntityFrameworkCore;

namespace JanHofmeyerAdmin.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventRegistration> EventRegistrations { get; set; }
        public DbSet<GalleryItem> Gallery { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<VolunteerApplication> VolunteerApplications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}