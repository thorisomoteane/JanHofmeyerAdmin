using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JanHofmeyerAdmin.Data;
using JanHofmeyerAdmin.Models;
using Microsoft.EntityFrameworkCore;

namespace JanHofmeyerAdmin.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Where(e => !e.IsDeleted)
                .OrderByDescending(e => e.EventDate)
                .ToListAsync();

            return View(events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event evt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ensure all required fields have values (not null)
                    evt.ShortSummary = string.IsNullOrEmpty(evt.ShortSummary) ? "" : evt.ShortSummary;
                    evt.Location = string.IsNullOrEmpty(evt.Location) ? "" : evt.Location;
                    evt.EventTime = string.IsNullOrEmpty(evt.EventTime) ? "" : evt.EventTime;
                    evt.GoalsAchieved = string.IsNullOrEmpty(evt.GoalsAchieved) ? "" : evt.GoalsAchieved;

                    // Make sure Category and Tag have values
                    if (string.IsNullOrEmpty(evt.Category))
                        evt.Category = "Registration Open";

                    if (string.IsNullOrEmpty(evt.Tag))
                        evt.Tag = "Social";

                    evt.CreatedDate = DateTime.Now;
                    evt.IsDeleted = false;

                    _context.Events.Add(evt);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Event created successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error creating event: {ex.Message}. Inner: {ex.InnerException?.Message}";
                }
            }
            else
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                TempData["Error"] = $"Validation failed: {errors}";
            }

            return View(evt);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt == null || evt.IsDeleted)
                return NotFound();

            return View(evt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event evt)
        {
            if (id != evt.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEvent = await _context.Events.FindAsync(id);
                    if (existingEvent == null)
                        return NotFound();

                    // Update fields
                    existingEvent.Title = evt.Title;
                    existingEvent.ShortSummary = string.IsNullOrEmpty(evt.ShortSummary) ? "" : evt.ShortSummary;
                    existingEvent.Category = string.IsNullOrEmpty(evt.Category) ? "Registration Open" : evt.Category;
                    existingEvent.Tag = string.IsNullOrEmpty(evt.Tag) ? "Social" : evt.Tag;
                    existingEvent.EventDate = evt.EventDate;
                    existingEvent.EventTime = string.IsNullOrEmpty(evt.EventTime) ? "" : evt.EventTime;
                    existingEvent.Location = string.IsNullOrEmpty(evt.Location) ? "" : evt.Location;
                    existingEvent.ExpectedAttendees = evt.ExpectedAttendees;
                    existingEvent.ActualAttendees = evt.ActualAttendees;
                    existingEvent.GoalsAchieved = string.IsNullOrEmpty(evt.GoalsAchieved) ? "" : evt.GoalsAchieved;
                    existingEvent.IsUpcoming = evt.IsUpcoming;

                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Event updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error updating event: {ex.Message}";
                }
            }

            return View(evt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var evt = await _context.Events.FindAsync(id);
            if (evt != null)
            {
                evt.IsDeleted = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Event deleted successfully!";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Registrations(int id)
        {
            var registrations = await _context.EventRegistrations
                .Include(r => r.Event)
                .Where(r => r.EventId == id)
                .OrderByDescending(r => r.RegistrationDate)
                .ToListAsync();

            ViewBag.EventTitle = registrations.FirstOrDefault()?.Event?.Title ?? "Event";
            return View(registrations);
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }

        public async Task<IActionResult> TestEventDb()
        {
            try
            {
                var testEvent = new Event
                {
                    Title = "Test Event",
                    ShortSummary = "",
                    Category = "Registration Open",
                    Tag = "Health",
                    EventDate = DateTime.Now.AddDays(30),
                    EventTime = "",
                    Location = "",
                    ExpectedAttendees = 50,
                    IsUpcoming = true,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    GoalsAchieved = "",
                    ActualAttendees = null
                };

                _context.Events.Add(testEvent);
                var result = await _context.SaveChangesAsync();

                return Content($"Success! Event saved with ID: {testEvent.Id}", "text/html");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}<br/>Inner: {ex.InnerException?.Message}", "text/html");
            }
        }
    }
}