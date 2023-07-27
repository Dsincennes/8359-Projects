using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LabFive.Models;
using Azure.Storage.Blobs;

namespace LabFive.Pages.Predictions
{
    public class DeleteModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        private readonly LabFive.Data.PredictionDataContext _context;

        public DeleteModel(LabFive.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
      public Prediction Prediction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Predictions == null)
            {
                return NotFound();
            }

            var prediction = await _context.Predictions.FirstOrDefaultAsync(m => m.PredictionId == id);

            if (prediction == null)
            {
                return NotFound();
            }
            else 
            {
                Prediction = prediction;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Predictions == null)
            {
                return NotFound();
            }


            var prediction = await _context.Predictions.FindAsync(id);

            if (prediction != null)
            {
                Prediction = prediction;
                _context.Predictions.Remove(Prediction);
                await _context.SaveChangesAsync();

                var containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
                var blobClient = _blobServiceClient.GetBlobContainerClient(containerName)
                                    .GetBlobClient(Prediction.FileName);

                await blobClient.DeleteIfExistsAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
