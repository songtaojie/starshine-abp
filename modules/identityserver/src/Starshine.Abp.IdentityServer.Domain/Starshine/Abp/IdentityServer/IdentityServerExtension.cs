using Starshine.Abp.IdentityServer.Entities;

namespace Starshine.Abp.IdentityServer
{
    internal static class IdentityServerExtension
    {
        #region PersistedGrant
        internal static PersistedGrant ToPersistedGrantEntity(this Starshine.IdentityServer.Models.PersistedGrant grant, PersistedGrant? persistedGrant = null)
        {
            if (persistedGrant == null)
            {
                persistedGrant = new PersistedGrant()
                {
                    Key = grant.Key,
                    Type = grant.Type,
                    ClientId = grant.ClientId
                };
            }
            else
            {
                persistedGrant.Key = grant.Key;
                persistedGrant.Type = grant.Type;
            }
            persistedGrant.SubjectId = grant.SubjectId;
            persistedGrant.SessionId = grant.SessionId;
            persistedGrant.ClientId = grant.ClientId;
            persistedGrant.Description = grant.Description;
            persistedGrant.CreationTime = grant.CreationTime;
            persistedGrant.Expiration = grant.Expiration;
            persistedGrant.ConsumedTime = grant.ConsumedTime;
            persistedGrant.Data = grant.Data;
            return persistedGrant;
        }

        internal static Starshine.IdentityServer.Models.PersistedGrant ToPersistedGrantModel(this PersistedGrant persistedGrant)
        {
            return new Starshine.IdentityServer.Models.PersistedGrant
            {
                ClientId = persistedGrant.ClientId ?? string.Empty,
                Data = persistedGrant.Data,
                Description = persistedGrant.Description,
                Expiration = persistedGrant.Expiration,
                Key = persistedGrant.Key,
                SessionId = persistedGrant.SessionId ?? string.Empty,
                SubjectId = persistedGrant.SubjectId ?? string.Empty,
                Type = persistedGrant.Type,
                CreationTime = persistedGrant.CreationTime,
                ConsumedTime = persistedGrant.ConsumedTime
            };
        }
        #endregion 

        #region Client
        internal static Starshine.IdentityServer.Models.Client ToClientModel(this Client client)
        {
            return new Starshine.IdentityServer.Models.Client
            {
                ClientId = client.ClientId,
                AbsoluteRefreshTokenLifetime = client.AbsoluteRefreshTokenLifetime,
                AccessTokenLifetime = client.AccessTokenLifetime,
                AccessTokenType = (Starshine.IdentityServer.Models.AccessTokenType)client.AccessTokenType,
                AllowAccessTokensViaBrowser = client.AllowAccessTokensViaBrowser,
                AllowedCorsOrigins = client.AllowedCorsOrigins.ConvertAll(r => r.Origin),
                AllowedGrantTypes = client.AllowedGrantTypes.ConvertAll(r => r.GrantType),
                AllowedIdentityTokenSigningAlgorithms = client.AllowedIdentityTokenSigningAlgorithms?.Split(",").ToList() ?? [],
                AllowedScopes = client.AllowedScopes.ConvertAll(r => r.Scope),
                ClientClaimsPrefix = client.ClientClaimsPrefix ?? string.Empty,
                AllowOfflineAccess = client.AllowOfflineAccess,
                AllowPlainTextPkce = client.AllowPlainTextPkce,
                AllowRememberConsent = client.AllowRememberConsent,
                AlwaysIncludeUserClaimsInIdToken = client.AlwaysIncludeUserClaimsInIdToken,
                AlwaysSendClientClaims = client.AlwaysSendClientClaims,
                AuthorizationCodeLifetime = client.AuthorizationCodeLifetime,
                BackChannelLogoutSessionRequired = client.BackChannelLogoutSessionRequired,
                BackChannelLogoutUri = client.BackChannelLogoutUri,
                Claims = client.Claims.ConvertAll(r => new Starshine.IdentityServer.Models.ClientClaim(r.Type, r.Value, System.Security.Claims.ClaimValueTypes.String)),
                ClientName = client.ClientName,
                ClientUri = client.ClientUri,
                ClientSecrets = client.ClientSecrets.ConvertAll(r => new Starshine.IdentityServer.Models.Secret(r.Value, r.Description, r.Expiration)),
                Description = client.Description,
                ConsentLifetime = client.ConsentLifetime,
                DeviceCodeLifetime = client.DeviceCodeLifetime,
                Enabled = client.Enabled,
                EnableLocalLogin = client.EnableLocalLogin,
                FrontChannelLogoutSessionRequired = client.FrontChannelLogoutSessionRequired,
                FrontChannelLogoutUri = client.FrontChannelLogoutUri,
                IdentityProviderRestrictions = client.IdentityProviderRestrictions.ConvertAll(r => r.Provider),
                IdentityTokenLifetime = client.IdentityTokenLifetime,
                IncludeJwtId = client.IncludeJwtId,
                LogoUri = client.LogoUri,
                PairWiseSubjectSalt = client.PairWiseSubjectSalt,
                PostLogoutRedirectUris = client.PostLogoutRedirectUris.ConvertAll(r => r.PostLogoutRedirectUri),
                Properties = client.Properties.ToDictionary(r => r.Key, r => r.Value),
                ProtocolType = client.ProtocolType,
                RequireClientSecret = client.RequireClientSecret,
                RedirectUris = client.RedirectUris.ConvertAll(r => r.RedirectUri),
                RefreshTokenExpiration = Starshine.IdentityServer.Models.TokenExpiration.Sliding,
                RequireConsent = client.RequireConsent,
                RequirePkce = client.RequirePkce,
                UserCodeType = client.UserCodeType,
                UserSsoLifetime = client.UserSsoLifetime,
                RefreshTokenUsage = Starshine.IdentityServer.Models.TokenUsage.OneTimeOnly,
                RequireRequestObject = client.RequireRequestObject,
                SlidingRefreshTokenLifetime = client.SlidingRefreshTokenLifetime,
                UpdateAccessTokenClaimsOnRefresh = client.UpdateAccessTokenClaimsOnRefresh,
            };
        }
        #endregion 

        #region IdentityResource
        internal static Starshine.IdentityServer.Models.IdentityResource ToIdentityResourceModel(this IdentityResource identityResource)
        {
            return new Starshine.IdentityServer.Models.IdentityResource
            {
                Description = identityResource.Description,
                DisplayName = identityResource.DisplayName ?? string.Empty,
                Emphasize = identityResource.Emphasize,
                Enabled = identityResource.Enabled, 
                Name = identityResource.Name,
                Required = identityResource.Required,
                ShowInDiscoveryDocument = identityResource.ShowInDiscoveryDocument,
                UserClaims = identityResource.UserClaims.ConvertAll(r => r.Type),
                Properties = identityResource.Properties.ToDictionary(r=>r.Key,r=>r.Value)
            };
        }

        internal static List<Starshine.IdentityServer.Models.IdentityResource> ToIdentityResourceModel(this List<IdentityResource> identityResources)
        {
            return identityResources.ConvertAll(r => r.ToIdentityResourceModel());
        }
        #endregion

        #region ApiScope
        internal static List<Starshine.IdentityServer.Models.ApiScope> ToApiScopeModel(this List<ApiScope> apiScopes)
        {
            return apiScopes.ConvertAll(r=> r.ToApiScopeModel());
        }

        internal static Starshine.IdentityServer.Models.ApiScope ToApiScopeModel(this ApiScope apiScope)
        {
            return new Starshine.IdentityServer.Models.ApiScope
            {
                Description = apiScope.Description,
                DisplayName = apiScope.DisplayName ?? string.Empty,
                Emphasize = apiScope.Emphasize,
                Enabled = apiScope.Enabled,
                Name = apiScope.Name,
                Required = apiScope.Required,
                ShowInDiscoveryDocument = apiScope.ShowInDiscoveryDocument,
                UserClaims = apiScope.UserClaims.ConvertAll(r => r.Type),
                Properties = apiScope.Properties.ToDictionary(r => r.Key, r => r.Value),
            };
        }
        #endregion

        #region ApiResource
        internal static List<Starshine.IdentityServer.Models.ApiResource> ToApiResourceModel(this List<ApiResource> apiResources)
        {
            return apiResources.ConvertAll(r => r.ToApiResourceModel());
        }

        internal static Starshine.IdentityServer.Models.ApiResource ToApiResourceModel(this ApiResource apiResource)
        {
            return new Starshine.IdentityServer.Models.ApiResource
            {
                Description = apiResource.Description,
                DisplayName = apiResource.DisplayName ?? string.Empty,
                Enabled = apiResource.Enabled,
                Name = apiResource.Name,
                ShowInDiscoveryDocument = apiResource.ShowInDiscoveryDocument,
                UserClaims = apiResource.UserClaims.ConvertAll(r => r.Type),
                Properties = apiResource.Properties.ToDictionary(r => r.Key, r => r.Value),
                AllowedAccessTokenSigningAlgorithms = apiResource.AllowedAccessTokenSigningAlgorithms?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList() ?? [],
                ApiSecrets = apiResource.Secrets.ConvertAll(r => new Starshine.IdentityServer.Models.Secret(r.Value,r.Description,r.Expiration)),
                Scopes = apiResource.Scopes.ConvertAll(r=>r.Scope)
            };
        }
        #endregion
    }
}
