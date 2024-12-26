using AutoMapper;

namespace Starshine.Abp.Identity;

public class AbpIdentityApplicationModuleAutoMapperProfile : Profile
{
    public AbpIdentityApplicationModuleAutoMapperProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties();

        CreateMap<IdentityRole, IdentityRoleDto>()
            .MapExtraProperties();
    }
}
