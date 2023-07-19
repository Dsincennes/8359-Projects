using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabFive.Models;
using Azure.Storage.Blobs;
using Azure;

namespace LabFive.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        private readonly LabFive.Data.PredictionDataContext _context;

        public CreateModel(LabFive.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Prediction Prediction { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Photo != null && Photo.Length > 0)
            {
                var containerName = Prediction.Question == Question.Earth ? earthContainerName : computerContainerName;
                try
                {
                    await _blobServiceClient.CreateBlobContainerAsync(containerName);
                }
                catch(RequestFailedException e)
                {
                }
                var fileName = Path.GetFileName(Photo.FileName);
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                using (var stream = Photo.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }
                Prediction.FileName = fileName;
                Prediction.Url = blobClient.Uri.ToString();
            }

            _context.Predictions.Add(Prediction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
