namespace GameStore.Api.ImageUpload;

public interface IUploadImage
{
    Task<string> UploadImageAsync(IFormFile file);
}