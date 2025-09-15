using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JanHofmeyerAdmin.Data;
using JanHofmeyerAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JanHofmeyerAdmin.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Create a list to hold our cleaned reviews
                var cleanReviews = new List<Review>();

                // Get raw data from database
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Reviews ORDER BY ReviewDate DESC";
                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var review = new Review
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ReviewerName = reader.IsDBNull(reader.GetOrdinal("ReviewerName")) ? "" : reader.GetString(reader.GetOrdinal("ReviewerName")),
                                ServiceUsed = reader.IsDBNull(reader.GetOrdinal("ServiceUsed")) ? "" : reader.GetString(reader.GetOrdinal("ServiceUsed")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                ReviewMessage = reader.IsDBNull(reader.GetOrdinal("ReviewMessage")) ? "" : reader.GetString(reader.GetOrdinal("ReviewMessage")),
                                ReviewDate = reader.GetDateTime(reader.GetOrdinal("ReviewDate")),
                                IsApproved = reader.GetBoolean(reader.GetOrdinal("IsApproved")),
                                IsDisplayed = reader.GetBoolean(reader.GetOrdinal("IsDisplayed"))
                            };
                            cleanReviews.Add(review);
                        }
                    }
                }

                return View(cleanReviews);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error loading reviews: {ex.Message}";
                return View(new List<Review>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Reviews SET IsApproved = 1, IsDisplayed = 1 WHERE Id = {0}", id);

                if (result > 0)
                {
                    TempData["Success"] = "Review approved and will be displayed on the website!";
                }
                else
                {
                    TempData["Error"] = "Review not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error approving review: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Hide(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Reviews SET IsDisplayed = 0 WHERE Id = {0}", id);

                if (result > 0)
                {
                    TempData["Success"] = "Review hidden from website!";
                }
                else
                {
                    TempData["Error"] = "Review not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error hiding review: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _context.Database.ExecuteSqlRawAsync(
                    "DELETE FROM Reviews WHERE Id = {0}", id);

                if (result > 0)
                {
                    TempData["Success"] = "Review deleted!";
                }
                else
                {
                    TempData["Error"] = "Review not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting review: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}