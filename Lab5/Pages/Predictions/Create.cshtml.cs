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


        public CreateModel(Lab5.Data.PredictionDataContext context, BlobServiceClient blobServiceClient)
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

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Process other form fields

                if (file != null && file.Length > 0)
                {
                    // Generate a unique file name or use the original file name as per your requirements
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Upload the file to the appropriate container using the BlobServiceClient
                    var containerClient = _blobServiceClient.GetBlobContainerClient("dsincenneslab5");
                    var blobClient = containerClient.GetBlobClient(fileName);
                    await blobClient.UploadAsync(file.OpenReadStream());

                    // Save the file URL or other relevant information to the database
                    // For example, you can set the Url property of the Prediction object
                    Prediction.FileName = fileName;
                    Prediction.Url = blobClient.Uri.ToString();
                }

                // Save the prediction to the database
                _context.Predictions.Add(Prediction);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
}
