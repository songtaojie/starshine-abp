using System.Threading.Tasks;
using System.Security.Principal;
using Starshine.IdentityServer.AspNetIdentity;
using Starshine.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityUser = Starshine.Abp.Identity.IdentityUser;
using Starshine.Abp.Identity.Managers;

namespace Starshine.Abp.IdentityServer.AspNetIdentity;

public class StarshineProfileService : ProfileService<IdentityUser>
{
    protected ICurrentTenant CurrentTenant { get; }

    public StarshineProfileService(
        IdentityUserManager userManager,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        ICurrentTenant currentTenant)
        : base(userManager, claimsFactory)
    {
        CurrentTenant = currentTenant;
    }

    [UnitOfWork]
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.GetProfileDataAsync(context);
        }
    }

    [UnitOfWork]
    public override async Task IsActiveAsync(IsActiveContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.IsActiveAsync(context);
        }
    }

    //[UnitOfWork]
    //public override Task<bool> IsUserActiveAsync(IdentityUser user)
    //{
    //    return Task.FromResult(user.IsActive);
    //}
}
