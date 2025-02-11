// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Azure.Messaging.EventGrid;
using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

public static class ImageResizer
{
    private static readonly string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

    [Function("ImageResizer")]
    public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
    {
        var blobUrl = eventGridEvent.Data.ToString();
        var blobUri = new Uri(blobUrl);
        var blobClient = new BlobClient(blobUri);

        using (var stream = new MemoryStream())
        {
            await blobClient.DownloadToAsync(stream);
            stream.Position = 0;

            using (var image = Image.Load(stream))
            {
                var sizes = new[] { 50, 100, 300 };

                foreach (var size in sizes)
                {
                    using (var resizedStream = new MemoryStream())
                    {
                        image.Mutate(x => x.Resize(size, size));
                        image.SaveAsJpeg(resizedStream);
                        resizedStream.Position = 0;

                        var resizedBlobClient = new BlobClient(storageConnectionString, blobClient.BlobContainerName, $"{Path.GetFileNameWithoutExtension(blobClient.Name)}_{size}.jpg");
                        await resizedBlobClient.UploadAsync(resizedStream, overwrite: true);
                    }
                }
            }
        }
    }
}