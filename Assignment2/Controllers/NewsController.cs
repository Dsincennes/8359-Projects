using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Azure.Storage.Blobs;
using Azure;

namespace Assignment2.Controllers
{
    public class NewsController : Controller
    {
        private readonly SportsDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string alphaContainerName = "alphaimages";
        private readonly string betaContainerName = "betaimages";
        private readonly string omegaContainerName = "omegaimages";


        public NewsController(SportsDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<IActionResult> Index(int? clubId, string sportClubFilter)
        {
            var newsList = await _context.News.ToListAsync();

            if (!string.IsNullOrEmpty(sportClubFilter))
            {
                var selectedSportClubCategory = Enum.Parse<SportClubs>(sportClubFilter);
                newsList = newsList.Where(news => news.SportClubs == selectedSportClubCategory).ToList();
            }

            return View(newsList);
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        [BindProperty]
        public News News { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NewsId,FileName,Url,SportClubs")] News news)
        {
            if (!ModelState.IsValid)
            {
                return View(news);
            }

            if (Photo != null && Photo.Length > 0)
            {
                var containerName = "";
                if (news.SportClubs == SportClubs.Alpha)
                {
                    containerName = alphaContainerName;
                }
                else if (news.SportClubs == SportClubs.Beta)
                {
                    containerName = betaContainerName;
                }
                else if (news.SportClubs == SportClubs.Omega)
                {
                    containerName = omegaContainerName;
                }

                try
                {
                    await _blobServiceClient.CreateBlobContainerAsync(containerName);
                }
                catch (RequestFailedException e)
                {
                    e.ToString();
                }
                var fileName = Path.GetFileName(Photo.FileName);
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                using (var stream = Photo.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                news.FileName = fileName;
                news.Url = blobClient.Uri.ToString();
            }

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "SportClubs"); // Redirect to SportClubs index
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,FileName,Url,SportClubs")] News news)
        {
            if (id != news.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.NewsId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'SportsDbContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
          return _context.News.Any(e => e.NewsId == id);
        }
    }
}
