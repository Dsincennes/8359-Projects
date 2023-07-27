using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabFive.Models;

namespace LabFive.Pages.Predictions
{
    public class IndexModel : PageModel
    {
        private readonly LabFive.Data.PredictionDataContext _context;

        public IndexModel(LabFive.Data.PredictionDataContext context)
        {
            _context = context;
        }

        public IList<Prediction> Prediction { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Predictions != null)
            {
                Prediction = await _context.Predictions.ToListAsync();
            }
        }
    }
}
