namespace GameStore.Api.Cors;

public static class CorsExtensions
{
    private const string allowedOriginSetting = "AllowedOrigin";

    public static IServiceCollection ConfigureCors(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                // I need to send these 3 things:
                // Access-Control-Allow-Origin: http://localhost:5000
                // Access-Control-Allow-Headers: content-type
                // Access-Control-Allow-Methods: POST

                // First, setting up the Origin
                var allowedOrigin = configuration[allowedOriginSetting] ?? throw new InvalidOperationException("AllowedOrigin is not set in appsettings.json file");

                // now setting up all three into the corsBuilder
                corsBuilder.WithOrigins(allowedOrigin) //Access-Control-Allow-Origin
                         .AllowAnyHeader() //Access-Control-Allow-Headers
                         .AllowAnyMethod() //Access-Control-Allow-Methods
                         .WithExposedHeaders("X-Pagination");
            });
        });

        return service;
    }

}