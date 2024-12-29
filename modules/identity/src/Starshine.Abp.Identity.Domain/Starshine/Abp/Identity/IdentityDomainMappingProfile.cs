using AutoMapper;
using Starshine.Abp.Users;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份域映射配置文件
/// </summary>
public class IdentityDomainMappingProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public IdentityDomainMappingProfile()
    {
        CreateMap<IdentityUser, UserEto>();
        CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
        CreateMap<IdentityRole, IdentityRoleEto>();
        CreateMap<OrganizationUnit, OrganizationUnitEto>();
    }
}
