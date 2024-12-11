using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using MotoRental.Core.Services;

namespace MotoRental.Infrastructure.ImageUploadService
{
    public class ImageUploadService : IImageUploadService
    {
        private readonly IConfiguration _configuration;
        public ImageUploadService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadBase64Image(string base64Image, string container)
        {
            var fileName = Guid.NewGuid().ToString() + ".jpg";

            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, ""); 
            
            byte[] imageBytes = Convert.FromBase64String(data);

            var azureBlobConnectionString = _configuration["AzureBlobService:ConnectionStrings"];

            var blobClient = new BlobClient(azureBlobConnectionString, container, fileName);

            using(var stream = new MemoryStream(imageBytes)) {
                await blobClient.UploadAsync(stream, overwrite: true);
            }
            
            return blobClient.Uri.AbsoluteUri;
        }
    }

}