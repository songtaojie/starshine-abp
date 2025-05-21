using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Starshine.Abp.OpenIddict.Controllers;

public partial class TokenController
{
    protected virtual async Task<IActionResult> HandleClientCredentialsAsync(OpenIddictRequest request)
    {
        // Note: the client credentials are automatically validated by OpenIddict:
        // if client_id or client_secret are invalid, this action won't be invoked.
        if(string.IsNullOrWhiteSpace(request.ClientId))
            throw new InvalidOperationException(L["TheApplicationDetailsCannotBeFound"]);
        var application = await ApplicationManager.FindByClientIdAsync(request.ClientId) 
            ?? throw new InvalidOperationException(L["TheApplicationDetailsCannotBeFound"]);

        // Create a new ClaimsIdentity containing the claims that
        // will be used to create an id_token, a token or a code.
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.PreferredUsername, OpenIddictConstants.Claims.Role);

        // The Subject and PreferredUsername will be removed by <see cref="RemoveClaimsFromClientCredentialsGrantType"/>.
        // Use the client_id as the subject identifier.
        var clientId = await ApplicationManager.GetClientIdAsync(application);
        identity.AddClaim(OpenIddictConstants.Claims.Subject, clientId ?? string.Empty);
        var displayName = await ApplicationManager.GetDisplayNameAsync(application);
        identity.AddClaim(OpenIddictConstants.Claims.PreferredUsername, displayName ?? string.Empty);

        // Note: In the original OAuth 2.0 specification, the client credentials grant
        // doesn't return an identity token, which is an OpenID Connect concept.
        //
        // As a non-standardized extension, OpenIddict allows returning an id_token
        // to convey information about the client application when the "openid" scope
        // is granted (i.e specified when calling principal.SetScopes()). When the "openid"
        // scope is not explicitly set, no identity token is returned to the client application.

        // Set the list of scopes granted to the client application in access_token.
        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(request.GetScopes());
        principal.SetResources(await GetResourcesAsync(request.GetScopes()));

        await OpenIddictClaimsPrincipalManager.HandleAsync(request, principal);

        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}
