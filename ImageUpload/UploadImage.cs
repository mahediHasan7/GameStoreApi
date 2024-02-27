using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GameStore.Api.ImageUpload;

public class UploadImage(BlobContainerClient containerClient) : IUploadImage
{
    private readonly BlobContainerClient containerClient = containerClient;

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        // making the container in the Azure Blob Storage where I will save the Blobs.
        // PublicAccessType.Blob means only the Blob is open for public access not the container itself
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        // creating the BlobClient (image)
        var blobClient = containerClient.GetBlobClient(file.FileName);

        // delete to avoid creating exact same named blob
        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync(); // delete to avoid creating exact same named blob
        }

        // Upload the file/images to the blob container
        using var fileStream = file.OpenReadStream();

        // I am setting the ContentType header to the content type of the file. This tells Azure Blob Storage what type of data the blob contains, which can be used by applications to determine how to process the blob.
        await blobClient.UploadAsync(
            fileStream,
            new BlobHttpHeaders { ContentType = file.ContentType }
        );

        // returning the url of the uploaded image
        return blobClient.Uri.ToString();
    }

}