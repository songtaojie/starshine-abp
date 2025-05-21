using AutoMapper;
using Starshine.Abp.Account.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;
using Volo.Abp.Account;

namespace Starshine.Abp.Account.Web;

public class StarshineAccountWebAutomapperProfile : Profile
{
    public StarshineAccountWebAutomapperProfile()
    {
        CreateMap<ProfileDto, AccountProfilePersonalInfoManagementGroupViewComponent.PersonalInfoModel>()
            .MapExtraProperties();
    }
}
