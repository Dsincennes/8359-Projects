﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;

namespace Lab4.Controllers
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
                var viewModel = new NewsViewModel
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
                var viewModel = new NewsViewModel
                {
                    SportClubs = await _context.SportClubs.ToListAsync(),
                    Fans = await _context.Fans.ToListAsync(),
                    Subscriptions = await _context.Subscriptions.ToListAsync(),
                    SelectedSportClubId = id,
                };
                return View(viewModel);
            }

        }

        // GET: SportClubs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.SportClubs == null)
            {
                return NotFound();
            }

            var sportClub = await _context.SportClubs
                .FirstOrDefaultAsync(m => m.ID == id);
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
        public async Task<IActionResult> Create([Bind("ID,Title,Fee")] SportClub sportClub)
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
        public async Task<IActionResult> Edit(string id, [Bind("ID,Title,Fee")] SportClub sportClub)
        {
            if (id != sportClub.ID)
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
                    if (!SportClubExists(sportClub.ID))
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
                .FirstOrDefaultAsync(m => m.ID == id);
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
            if (sportClub != null)
            {
                _context.SportClubs.Remove(sportClub);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SportClubExists(string id)
        {
            return _context.SportClubs.Any(e => e.ID == id);
        }
    }
}
