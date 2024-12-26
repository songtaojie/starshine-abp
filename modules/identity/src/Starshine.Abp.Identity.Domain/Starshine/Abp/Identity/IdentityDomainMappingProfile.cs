using AutoMapper;
using Volo.Abp.Users;

namespace Starshine.Abp.Identity;

public class IdentityDomainMappingProfile : Profile
{
    public IdentityDomainMappingProfile()
    {
        CreateMap<IdentityUser, UserEto>();
        CreateMap<IdentityClaimType, IdentityClaimTypeEto>();
        CreateMap<IdentityRole, IdentityRoleEto>();
        CreateMap<OrganizationUnit, OrganizationUnitEto>();
    }
}
