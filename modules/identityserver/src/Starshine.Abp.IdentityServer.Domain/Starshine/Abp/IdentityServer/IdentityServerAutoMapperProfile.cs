using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Starshine.Abp.IdentityServer.ApiScopes;
using Starshine.Abp.IdentityServer.Clients;
using Starshine.Abp.IdentityServer.Devices;
using Starshine.Abp.IdentityServer.Entities;
using Starshine.Abp.IdentityServer.Events;
using Starshine.Abp.IdentityServer.Grants;
using Starshine.Abp.IdentityServer.IdentityResources;

namespace Starshine.Abp.IdentityServer;

public class IdentityServerAutoMapperProfile : Profile
{
    /// <summary>
    /// TODO: Reverse maps will not used probably. Remove those will not used
    /// </summary>
    public IdentityServerAutoMapperProfile()
    {
        CreateMap<UserClaim, string>()
            .ConstructUsing(src => src.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateClientMap();
        CreateApiResourceMap();
        CreateApiScopeMap();
        CreateIdentityResourceMap();
        CreatePersistedGrantMap();
        CreateDeviceFlowCodesMap();
    }

    private void CreateClientMap()
    {
        CreateMap<ClientCorsOrigin, string>()
            .ConstructUsing(src => src.Origin)
            .ReverseMap()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

        CreateMap<ClientProperty, KeyValuePair<string, string>>()
         .ReverseMap();

        CreateMap<Client, Starshine.IdentityServer.Models.Client>()
            .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
            .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms))
            .ReverseMap()
            .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms));

        CreateMap<ClientCorsOrigin, string>()
            .ConstructUsing(src => src.Origin)
            .ReverseMap()
            .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

        CreateMap<ClientIdPRestriction, string>()
            .ConstructUsing(src => src.Provider)
            .ReverseMap()
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

        CreateMap<ClientClaim, Claim>(MemberList.None)
            .ConstructUsing(src => new Claim(src.Type, src.Value))
            .ReverseMap();

        CreateMap<ClientClaim, Starshine.IdentityServer.Models.ClientClaim>(MemberList.None)
            .ConstructUsing(src => new Starshine.IdentityServer.Models.ClientClaim(src.Type, src.Value, ClaimValueTypes.String))
            .ReverseMap();

        CreateMap<ClientScope, string>()
            .ConstructUsing(src => src.Scope)
            .ReverseMap()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

        CreateMap<ClientPostLogoutRedirectUri, string>()
            .ConstructUsing(src => src.PostLogoutRedirectUri)
            .ReverseMap()
            .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

        CreateMap<ClientRedirectUri, string>()
            .ConstructUsing(src => src.RedirectUri)
            .ReverseMap()
            .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

        CreateMap<ClientGrantType, string>()
            .ConstructUsing(src => src.GrantType)
            .ReverseMap()
            .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

        CreateMap<ClientSecret, Starshine.IdentityServer.Models.Secret>(MemberList.Destination)
            .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
            .ReverseMap();

        CreateMap<Client, ClientEto>();
    }

    private void CreateApiResourceMap()
    {
        CreateMap<ApiResource, Starshine.IdentityServer.Models.ApiResource>()
            .ForMember(dest => dest.ApiSecrets, opt => opt.MapFrom(src => src.Secrets))
            .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms));

        CreateMap<ApiResourceSecret, Starshine.IdentityServer.Models.Secret>();

        CreateMap<ApiResourceScope, string>()
            .ConstructUsing(x => x.Scope)
            .ReverseMap()
            .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

        CreateMap<ApiResourceProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<ApiResource, ApiResourceEto>();
    }

    private void CreateApiScopeMap()
    {
        CreateMap<ApiScopeProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<ApiScopeClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateMap<ApiScope, Starshine.IdentityServer.Models.ApiScope>(MemberList.Destination)
            .ConstructUsing(src => new Starshine.IdentityServer.Models.ApiScope())
            .ReverseMap();
    }

    private void CreateIdentityResourceMap()
    {
        CreateMap<IdentityResource, Starshine.IdentityServer.Models.IdentityResource>()
            .ConstructUsing(src => new Starshine.IdentityServer.Models.IdentityResource());

        CreateMap<IdentityResourceClaim, string>()
            .ConstructUsing(x => x.Type)
            .ReverseMap()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        CreateMap<IdentityResourceProperty, KeyValuePair<string, string>>()
            .ReverseMap();

        CreateMap<IdentityResource, IdentityResourceEto>();
    }

    private void CreatePersistedGrantMap()
    {
        //TODO: Why PersistedGrant mapping is in this profile?
        CreateMap<PersistedGrant, Starshine.IdentityServer.Models.PersistedGrant>().ReverseMap();
        CreateMap<PersistedGrant, PersistedGrantEto>();
    }

    private void CreateDeviceFlowCodesMap()
    {
        CreateMap<DeviceFlowCodes, DeviceFlowCodesEto>();
    }
}
