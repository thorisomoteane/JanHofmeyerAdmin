using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JanHofmeyerAdmin.Data;
using JanHofmeyerAdmin.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JanHofmeyerAdmin.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dashboard = new DashboardViewModel
            {
                TotalProjects = await _context.Projects.CountAsync(p => !p.IsDeleted),
                ActiveEvents = await _context.Events.CountAsync(e => !e.IsDeleted && e.IsUpcoming),
                PendingReviews = await _context.Reviews.CountAsync(r => !r.IsApproved),
                NewMessages = await _context.ContactMessages.CountAsync(m => !m.IsRead),
                PendingVolunteers = await _context.VolunteerApplications
                    .CountAsync(v => v.Status == "Pending"),
                TotalGalleryItems = await _context.Gallery.CountAsync(g => !g.IsDeleted),
                RecentRegistrations = await _context.EventRegistrations
                    .Where(r => r.RegistrationDate >= DateTime.Now.AddDays(-7))
                    .CountAsync()
            };

            return View(dashboard);
        }
    }
}