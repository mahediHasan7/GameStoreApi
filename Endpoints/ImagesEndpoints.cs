using Asp.Versioning.Builder;
using GameStore.Api.Authorization;
using GameStore.Api.Dto;
using GameStore.Api.ImageUpload;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameStore.Api.Endpoints;

public static class ImagesEndpoints
{
    public static IVersionedEndpointRouteBuilder MapImagesEndPoints(this IEndpointRouteBuilder routes)
    {
        var api = routes.NewVersionedApi();

        api.MapPost("/images", async Task<Results<Ok<UploadImageDto>, BadRequest>> (IFormFile file, IUploadImage uploadImage) =>
        {

            if (file == null || file.Length <= 0)
            {
                return TypedResults.BadRequest();
            }

            var imageUri = await uploadImage.UploadImageAsync(file);

            return TypedResults.Ok(new UploadImageDto(imageUri));
        })
        .RequireAuthorization(Policies.WriteAccess)
        .HasApiVersion(1.0)
        .WithSummary("Upload a file to storage")
        .WithDescription("...")
        .WithTags("images")
        .DisableAntiforgery();

        return api;
    }

}