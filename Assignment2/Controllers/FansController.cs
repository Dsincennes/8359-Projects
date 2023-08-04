using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment2.Data;
using Assignment2.Models;
using Assignment2.Models.ViewModels;

namespace Assignment2.Controllers
{
    public class FansController : Controller
    {
        private readonly SportsDbContext _context;

        public FansController(SportsDbContext context)
        {
            _context = context;
        }


        public IActionResult Index(string selectedFanId = "")
        {
            var fansWithSubscriptions = _context.Fans
                .Include(f => f.Subscriptions)
                .ThenInclude(s => s.SportClub)
                .ToList();

            var viewModelList = new List<FanSubscriptionViewModel>();

            foreach (var fan in fansWithSubscriptions)
            {
                var fanViewModel = new FanSubscriptionViewModel
                {
                    Fan = fan,
                    Subscriptions = fan.Subscriptions.Select(subscription => new SportClubSubscriptionViewModel
                    {
                        SportClubId = subscription.SportClub.Id,
                        Title = subscription.SportClub.Title,
                        IsMember = true
                    })
                };

                if (!string.IsNullOrEmpty(selectedFanId) && selectedFanId == fan.Id.ToString())
                {
                    fanViewModel.SelectedFanId = selectedFanId;
                }

                viewModelList.Add(fanViewModel);
            }

            return View(viewModelList);
        }

        // GET: Fans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // GET: Fans/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fan);
        }

        // GET: Fans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans.FindAsync(id);
            if (fan == null)
            {
                return NotFound();
            }
            return View(fan);
        }

        public IActionResult EditSubscription(int fanId)
        {
            var fan = _context.Fans
                .Include(f => f.Subscriptions)
                .ThenInclude(s => s.SportClub)
                .FirstOrDefault(f => f.Id == fanId);

            if (fan == null)
            {
                return NotFound();
            }

            var allSportClubs = _context.SportClubs.ToList(); // Get all sport clubs from the database

            var fanSubscriptionViewModel = new FanSubscriptionViewModel
            {
                Fan = fan,
                Subscriptions = fan.Subscriptions.Select(subscription => new SportClubSubscriptionViewModel
                {
                    SportClubId = subscription.SportClub.Id,
                    Title = subscription.SportClub.Title,
                    IsMember = true
                }),
                AllSubscriptions = allSportClubs.Select(sportClub => new SportClubSubscriptionViewModel
                {
                    SportClubId = sportClub.Id,
                    Title = sportClub.Title,
                    IsMember = fan.Subscriptions.Any(sub => sub.SportClubId == sportClub.Id)
                }),
                SelectedFanId = fan.Id.ToString()
            };

            return View("EditSubscription", fanSubscriptionViewModel);
        }

        [HttpPost]
        public IActionResult Register(int fanId, string sportClubId)
        {
            var fan = _context.Fans
            .Include(f => f.Subscriptions)
            .FirstOrDefault(f => f.Id == fanId);

            if (fan != null)
            {
                fan.Subscriptions.Add(new Subscription
                {
                    FanId = fan.Id,
                    SportClubId = sportClubId
                });

                _context.SaveChanges();
            }

            return EditSubscription(fanId); // Return the EditSubscription view with updated data
        }

        [HttpPost]
        public IActionResult Unregister(int fanId, string sportClubId)
        {
            var fan = _context.Fans
                .Include(f => f.Subscriptions)
                .FirstOrDefault(f => f.Id == fanId);

            if (fan != null)
            {
                var subscriptionToRemove = fan.Subscriptions.FirstOrDefault(s => s.SportClubId == sportClubId);
                if (subscriptionToRemove != null)
                {
                    fan.Subscriptions.Remove(subscriptionToRemove);
                    _context.SaveChanges();
                }
            }

            return EditSubscription(fanId); // Return the EditSubscription view with updated data
        }

        // POST: Fans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LastName,FirstName,BirthDate")] Fan fan)
        {
            if (id != fan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FanExists(fan.Id))
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
            return View(fan);
        }

        // GET: Fans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fans == null)
            {
                return NotFound();
            }

            var fan = await _context.Fans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fan == null)
            {
                return NotFound();
            }

            return View(fan);
        }

        // POST: Fans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fans == null)
            {
                return Problem("Entity set 'SportsDbContext.Fans'  is null.");
            }
            var fan = await _context.Fans.FindAsync(id);
            if (fan != null)
            {
                _context.Fans.Remove(fan);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FanExists(int id)
        {
          return _context.Fans.Any(e => e.Id == id);
        }
    }
}
