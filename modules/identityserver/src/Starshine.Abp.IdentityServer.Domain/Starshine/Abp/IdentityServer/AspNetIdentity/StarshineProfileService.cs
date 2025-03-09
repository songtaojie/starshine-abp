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
/// <summary>
/// IdentityServer 配置文件服务
/// </summary>
public class StarshineProfileService : ProfileService<IdentityUser>
{
    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="claimsFactory"></param>
    /// <param name="currentTenant"></param>
    public StarshineProfileService(
        IdentityUserManager userManager,
        IUserClaimsPrincipalFactory<IdentityUser> claimsFactory,
        ICurrentTenant currentTenant)
        : base(userManager, claimsFactory)
    {
        CurrentTenant = currentTenant;
    }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [UnitOfWork]
    public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        using (CurrentTenant.Change(context.Subject.FindTenantId()))
        {
            await base.GetProfileDataAsync(context);
        }
    }

    /// <summary>
    /// 获取用户是否激活
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
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
