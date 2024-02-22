using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace GameStore.Api.Authorization;

public class ScopeTransformation : IClaimsTransformation
{

    private const string scopeClaimName = "scope";

    // principal is an instance of ClaimsPrincipal. The ClaimsPrincipal class represents a security principal, which is an entity that can be authenticated by a security system. It holds all the security information for an entity, including claims. we can retrieve the claims using FindFirst method from the ClaimsPrincipal.
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var scopeClaim = principal.FindFirst(scopeClaimName); // "games: read games: write"

        if (scopeClaim is null)
        {
            return await Task.FromResult(principal);
        }

        var scopes = scopeClaim.Value.Split(" ");

        // the as keyword used for safe type casting. If the Identity property is not a ClaimsIdentity, 'as' will return null instead of throwing exception.
        var originalIdentity = principal.Identity as ClaimsIdentity;

        // identity (brand new one) will have the same claims as the original. We are creating a new ClaimsIdentity, because we want to modify the claims without affecting the original ClaimsIdentity.
        var identity = new ClaimsIdentity(originalIdentity);

        // now remove the string scope from the identity before we add our scopes
        var originalScopeClaim = identity.Claims.FirstOrDefault(claim => claim.Type == scopeClaimName);
        if (originalScopeClaim is not null)
        {
            identity.RemoveClaim(originalScopeClaim);
        }

        // now we can add our brand new scopes array, the Select will give us that.
        identity.AddClaims(scopes.Select(scope => new Claim(scopeClaimName, scope)));

        return await Task.FromResult(new ClaimsPrincipal(identity));
    }
}