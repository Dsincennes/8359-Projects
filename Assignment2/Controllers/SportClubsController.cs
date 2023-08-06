using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;

namespace Assignment2.Controllers
{
    public class SportClubsController : Controller
    {
        private readonly SportsDbContext _context;

        public SportClubsController(SportsDbContext context)
        {
            _context = context;
        }

        // GET: SportClubs
        public async Task<IActionResult> Index(string id)
        {

            if (!string.IsNullOrEmpty(id))
            {
                var viewModel = new SportClubViewModel
                {
                    SportClubs = await _context.SportClubs.ToListAsync(),
                    Fans = await _context.Fans.ToListAsync(),
                    Subscriptions = await _context.Subscriptions.ToListAsync(),
                    SelectedSportClubId = id
                };
                return View(viewModel);

            }
            else
            {
                id = string.Empty;
                var viewModel = new SportClubViewModel
                {
                    SportClubs = await _context.SportClubs.ToListAsync(),
                    Fans = await _context.Fans.ToListAsync(),
                    Subscriptions = await _context.Subscriptions.ToListAsync(),
                    SelectedSportClubId = id,
                };
                return View(viewModel);
            }

        }
        public async Task<IActionResult> News(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedSportClub = await _context.SportClubs.FindAsync(id);
            if (selectedSportClub == null)
            {
                return NotFound();
            }

            ViewData["SelectedSportClub"] = selectedSportClub;

            IQueryable<News> newsQuery = _context.News.Where(n => n.SportClubs.ToString() == selectedSportClub.Title);
            var newsItems = await newsQuery.ToListAsync();

            return RedirectToAction("Index", "News", new { sportClub = selectedSportClub.Title, area = "" });
        }

        // GET: SportClubs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportClub == null)
            {
                return NotFound();
            }

            return View(sportClub);
        }

        // GET: SportClubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportClubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Fee")] SportClub sportClub)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sportClub);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sportClub);
        }

        // GET: SportClubs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs.FindAsync(id);
            if (sportClub == null)
            {
                return NotFound();
            }
            return View(sportClub);
        }

        // POST: SportClubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Title,Fee")] SportClub sportClub)
        {
            if (id != sportClub.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportClub);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportClubExists(sportClub.Id))
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
            return View(sportClub);
        }

        // GET: SportClubs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sportClub == null)
            {
                return NotFound();
            }

            return View(sportClub);
        }

        // POST: SportClubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.SportClubs == null)
            {
                return Problem("Entity set 'SportsDbContext.SportClubs'  is null.");
            }

            var sportClub = await _context.SportClubs.FindAsync(id);

            var newsImages = await _context.News
                .Where(news => news.SportClubs == Enum.Parse<SportClubs>(sportClub.Title))
                .ToListAsync();

            if (newsImages.Count > 0)
            {
                TempData["ErrorMessage"] = "Cannot delete this Sport Club because it has associated news images.";
                return RedirectToAction(nameof(Index));
            }

            if (sportClub != null)
            {
                _context.SportClubs.Remove(sportClub);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportClubExists(string id)
        {
          return _context.SportClubs.Any(e => e.Id == id);
        }
    }
}
