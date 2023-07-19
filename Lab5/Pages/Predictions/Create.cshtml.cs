using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5.Data;
using Lab5.Models;
using Azure.Storage.Blobs;

namespace Lab5.Pages.Predictions
{
    public class CreateModel : PageModel
    {
        private readonly Lab5.Data.PredictionDataContext _context;

        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";

        [BindProperty]
        public Prediction Prediction { get; set; }

        public CreateModel(PredictionDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (file != null)
            {
                // Generate a unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

                // Upload the image to Azure Blob Storage
                var containerClient = _blobServiceClient.GetBlobContainerClient("dsincenneslab5");
                var blobClient = containerClient.GetBlobClient(uniqueFileName);

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    await blobClient.UploadAsync(memoryStream, overwrite: true);
                }

                // Set the FileName and Url properties of the Prediction model
                Prediction.FileName = uniqueFileName;
                Prediction.Url = blobClient.Uri.ToString();
            }

            _context.Predictions.Add(Prediction);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}