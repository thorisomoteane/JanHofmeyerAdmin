using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JanHofmeyerAdmin.Data;
using JanHofmeyerAdmin.Models;
using JanHofmeyerAdmin.Services;
using Microsoft.EntityFrameworkCore;

namespace JanHofmeyerAdmin.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AzureStorageService _storageService;

        public ProjectsController(ApplicationDbContext context, AzureStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();

            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure no null values for required database fields
                    project.AboutSection = project.AboutSection ?? "";
                    project.KeyGoals = project.KeyGoals ?? "";
                    project.ShortSummary = project.ShortSummary ?? "";
                    project.ImageUrl = "";

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        project.ImageUrl = await UploadImage(imageFile);
                    }

                    project.CreatedDate = DateTime.Now;
                    project.IsDeleted = false;

                    _context.Projects.Add(project);
                    var result = await _context.SaveChangesAsync();

                    TempData["Success"] = "Project created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error: {ex.Message}";
                }
            }

            return View(project);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null || project.IsDeleted)
                return NotFound();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project, IFormFile imageFile)
        {
            if (id != project.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProject = await _context.Projects.FindAsync(id);
                    if (existingProject == null)
                        return NotFound();

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        existingProject.ImageUrl = await UploadImage(imageFile);
                    }

                    existingProject.Title = project.Title;
                    existingProject.ShortSummary = project.ShortSummary;
                    existingProject.Status = project.Status;
                    existingProject.Theme = project.Theme;
                    existingProject.PeopleImpacted = project.PeopleImpacted;
                    existingProject.AboutSection = project.AboutSection;
                    existingProject.KeyGoals = project.KeyGoals;

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Project updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.Id))
                        return NotFound();
                    throw;
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error updating project: {ex.Message}";
                }
            }

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project != null)
                {
                    project.IsDeleted = true;
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Project deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting project: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }

        private async Task<string> UploadImage(IFormFile file)
        {
            return await _storageService.UploadFileAsync(file);
        }

        public async Task<IActionResult> TestDb()
        {
            try
            {
                // Test connection
                var canConnect = await _context.Database.CanConnectAsync();

                // Count existing records
                var projectCount = await _context.Projects.CountAsync();
                var eventCount = await _context.Events.CountAsync();

                // Try to add a test project
                var testProject = new Project
                {
                    Title = $"Test Project {DateTime.Now.Ticks}",
                    ShortSummary = "Test",
                    Status = "Active",
                    Theme = "Test",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.Projects.Add(testProject);
                var saveResult = await _context.SaveChangesAsync();

                return Content($@"
            <h3>Database Test Results:</h3>
            Can Connect: {canConnect}<br/>
            Projects in DB: {projectCount}<br/>
            Events in DB: {eventCount}<br/>
            Test Save Result: {saveResult} records saved<br/>
            Test Project ID: {testProject.Id}<br/>
            Connection String: {_context.Database.GetConnectionString()?.Substring(0, 50)}...<br/>
        ", "text/html");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}<br/>Inner: {ex.InnerException?.Message}", "text/html");
            }
        }
    }
}