namespace GameStore.Api.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddGameAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
                .AddPolicy(Policies.ReadAccess, policy => policy.RequireClaim("scope", "games:read"))
                .AddPolicy(Policies.WriteAccess, policy => policy
                    .RequireClaim("scope", "games:write")
                    .RequireRole("Admin"));

        return services;
    }
}