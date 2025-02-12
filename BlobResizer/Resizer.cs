using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;

namespace BlobResizer
{
    public class Resizer
    {
        private static readonly string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
        private readonly ILogger<Resizer> _logger;

        public Resizer(ILogger<Resizer> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Resizer))]
        public async Task Run(
            [BlobTrigger("profile-pictures/original/{name}", Connection = "AzureWebJobsStorage")] Stream stream,
            string name)
        {
            _logger.LogInformation($"Resizer started for blob: {name}");

            try
            {
                if (IsAlreadyProcessed(name))
                {
                    _logger.LogInformation("Blob '{name}' is already processed. Exiting.", name);
                    return;
                }

                stream.Position = 0;

                using Image image = Image.Load(stream);
                int[] sizes = [50, 100, 300];

                foreach (int size in sizes)
                {
                    using Image resizedImage = ResizeImage(image, size);
                    await UploadResizedImageAsync(resizedImage, size, name);
                    _logger.LogInformation($"Uploaded resized image with size {size} for blob '{name}'.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing blob '{name}'");
                throw;
            }
        }

        private bool IsAlreadyProcessed(string blobName)
        {
            return blobName.Contains("_50") || blobName.Contains("_100") || blobName.Contains("_300");
        }

        private Image ResizeImage(Image image, int size)
        {
            return image.Clone(ctx => ctx.Resize(size, size));
        }

        private async Task UploadResizedImageAsync(Image image, int size, string originalName)
        {
            string containerName = "profile-pictures";
            string originalFileName = Path.GetFileNameWithoutExtension(originalName);
            string resizedBlobName = $"resized/{originalFileName}_{size}.jpg";

            using MemoryStream resizedStream = new();
            image.SaveAsJpeg(resizedStream);
            resizedStream.Position = 0;

            BlobClient blobClient = new BlobClient(storageConnectionString, containerName, resizedBlobName);
            await blobClient.UploadAsync(resizedStream, overwrite: true);
        }
    }
}
