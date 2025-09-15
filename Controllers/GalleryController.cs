using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using JanHofmeyerAdmin.Data;
using JanHofmeyerAdmin.Models;
using JanHofmeyerAdmin.Services;
using Microsoft.EntityFrameworkCore;

namespace JanHofmeyerAdmin.Controllers
{
    [Authorize]
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AzureStorageService _storageService;

        public GalleryController(ApplicationDbContext context, AzureStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _context.Gallery
                .Where(g => !g.IsDeleted)
                .OrderByDescending(g => g.UploadDate)
                .ToListAsync();

            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(GalleryItem item, IFormFile mediaFile)
        {
            if (mediaFile != null && mediaFile.Length > 0)
            {
                try
                {
                    // Upload to Azure Storage
                    item.MediaUrl = await _storageService.UploadFileAsync(mediaFile);
                    item.UploadDate = DateTime.Now;

                    _context.Gallery.Add(item);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Media uploaded successfully!";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Upload failed: {ex.Message}";
                }
            }
            else
            {
                TempData["Error"] = "Please select a file to upload.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Gallery.FindAsync(id);
            if (item != null)
            {
                item.IsDeleted = true;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Gallery item deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}