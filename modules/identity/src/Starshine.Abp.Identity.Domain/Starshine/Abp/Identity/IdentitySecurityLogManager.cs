using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;
using Volo.Abp.SecurityLog;
using Volo.Abp.Users;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志管理器
/// </summary>
public class IdentitySecurityLogManager : ITransientDependency
{
    /// <summary>
    /// 安全日志管理器
    /// </summary>
    protected ISecurityLogManager SecurityLogManager { get; }
    /// <summary>
    /// 用户管理器
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// 当前主要访问者
    /// </summary>
    protected ICurrentPrincipalAccessor CurrentPrincipalAccessor { get; }
    /// <summary>
    /// 用户声明主体工厂
    /// </summary>
    protected IUserClaimsPrincipalFactory<IdentityUser> UserClaimsPrincipalFactory { get; }
    /// <summary>
    /// CurrentUser
    /// </summary>
    protected ICurrentUser CurrentUser { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="securityLogManager"></param>
    /// <param name="userManager"></param>
    /// <param name="currentPrincipalAccessor"></param>
    /// <param name="userClaimsPrincipalFactory"></param>
    /// <param name="currentUser"></param>
    public IdentitySecurityLogManager(
        ISecurityLogManager securityLogManager,
        IdentityUserManager userManager,
        ICurrentPrincipalAccessor currentPrincipalAccessor,
        IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
        ICurrentUser currentUser)
    {
        SecurityLogManager = securityLogManager;
        UserManager = userManager;
        CurrentPrincipalAccessor = currentPrincipalAccessor;
        UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
        CurrentUser = currentUser;
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task SaveAsync(IdentitySecurityLogContext context)
    {
        Action<SecurityLogInfo> securityLogAction = securityLog =>
        {
            securityLog.Identity = context.Identity;
            securityLog.Action = context.Action;

            if (!context.UserName.IsNullOrWhiteSpace())
            {
                securityLog.UserName = context.UserName;
            }

            if (!context.ClientId.IsNullOrWhiteSpace())
            {
                securityLog.ClientId = context.ClientId;
            }

            foreach (var property in context.ExtraProperties)
            {
                securityLog.ExtraProperties[property.Key] = property.Value;
            }
        };

        if (CurrentUser.IsAuthenticated)
        {
            await SecurityLogManager.SaveAsync(securityLogAction);
        }
        else
        {
            if (context.UserName.IsNullOrWhiteSpace())
            {
                await SecurityLogManager.SaveAsync(securityLogAction);
            }
            else
            {
                var user = await UserManager.FindByNameAsync(context.UserName);
                if (user != null)
                {
                    using (CurrentPrincipalAccessor.Change(await UserClaimsPrincipalFactory.CreateAsync(user)))
                    {
                        await SecurityLogManager.SaveAsync(securityLogAction);
                    }
                }
                else
                {
                    await SecurityLogManager.SaveAsync(securityLogAction);
                }
            }
        }
    }
}
