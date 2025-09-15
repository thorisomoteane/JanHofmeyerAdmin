using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JanHofmeyerAdmin.Data;
using JanHofmeyerAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JanHofmeyerAdmin.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Contact()
        {
            try
            {
                var messages = new List<ContactMessage>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT * FROM ContactMessages ORDER BY MessageDate DESC";
                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var message = new ContactMessage
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? "" : reader.GetString(reader.GetOrdinal("FullName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? "" : reader.GetString(reader.GetOrdinal("Phone")),
                                Subject = reader.IsDBNull(reader.GetOrdinal("Subject")) ? "" : reader.GetString(reader.GetOrdinal("Subject")),
                                Message = reader.IsDBNull(reader.GetOrdinal("Message")) ? "" : reader.GetString(reader.GetOrdinal("Message")),
                                MessageDate = reader.GetDateTime(reader.GetOrdinal("MessageDate")),
                                IsRead = reader.GetBoolean(reader.GetOrdinal("IsRead"))
                            };

                            // Handle InquiryType if it exists
                            try
                            {
                                var inquiryTypeOrdinal = reader.GetOrdinal("InquiryType");
                                message.InquiryType = reader.IsDBNull(inquiryTypeOrdinal) ? "General" : reader.GetString(inquiryTypeOrdinal);
                            }
                            catch
                            {
                                message.InquiryType = "General";
                            }

                            messages.Add(message);
                        }
                    }
                }

                return View(messages);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading messages: {ex.Message}";
                return View(new List<ContactMessage>());
            }
        }

        public async Task<IActionResult> Volunteers()
        {
            try
            {
                var applications = new List<VolunteerApplication>();

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT * FROM VolunteerApplications ORDER BY ApplicationDate DESC";
                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var app = new VolunteerApplication
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FullName = reader.IsDBNull(reader.GetOrdinal("FullName")) ? "" : reader.GetString(reader.GetOrdinal("FullName")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString(reader.GetOrdinal("Email")),
                                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? "" : reader.GetString(reader.GetOrdinal("Phone")),
                                AgeGroup = reader.IsDBNull(reader.GetOrdinal("AgeGroup")) ? "" : reader.GetString(reader.GetOrdinal("AgeGroup")),
                                Availability = reader.IsDBNull(reader.GetOrdinal("Availability")) ? "" : reader.GetString(reader.GetOrdinal("Availability")),
                                AreasOfInterest = reader.IsDBNull(reader.GetOrdinal("AreasOfInterest")) ? "" : reader.GetString(reader.GetOrdinal("AreasOfInterest")),
                                RelevantExperience = reader.IsDBNull(reader.GetOrdinal("RelevantExperience")) ? "" : reader.GetString(reader.GetOrdinal("RelevantExperience")),
                                ApplicationDate = reader.GetDateTime(reader.GetOrdinal("ApplicationDate")),
                                Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Pending" : reader.GetString(reader.GetOrdinal("Status"))
                            };

                            // Handle optional fields
                            try
                            {
                                var reviewedDateOrdinal = reader.GetOrdinal("ReviewedDate");
                                app.ReviewedDate = reader.IsDBNull(reviewedDateOrdinal) ? null : (DateTime?)reader.GetDateTime(reviewedDateOrdinal);
                            }
                            catch
                            {
                                app.ReviewedDate = null;
                            }

                            try
                            {
                                var adminNotesOrdinal = reader.GetOrdinal("AdminNotes");
                                app.AdminNotes = reader.IsDBNull(adminNotesOrdinal) ? "" : reader.GetString(adminNotesOrdinal);
                            }
                            catch
                            {
                                app.AdminNotes = "";
                            }

                            applications.Add(app);
                        }
                    }
                }

                return View(applications);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading volunteer applications: {ex.Message}";
                return View(new List<VolunteerApplication>());
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE ContactMessages SET IsRead = 1 WHERE Id = {0}", id);
                TempData["Success"] = "Message marked as read.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Contact");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVolunteerStatus(int id, string status)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE VolunteerApplications SET Status = {0}, ReviewedDate = {1} WHERE Id = {2}",
                    status, DateTime.Now, id);

                TempData["Success"] = $"Volunteer application {status}!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("Volunteers");
        }
    }
}