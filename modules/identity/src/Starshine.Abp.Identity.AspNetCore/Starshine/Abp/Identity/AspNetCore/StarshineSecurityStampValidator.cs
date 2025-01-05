using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity.AspNetCore;
/// <summary>
/// 安全印章验证器
/// </summary>
public class StarshineSecurityStampValidator : SecurityStampValidator<IdentityUser>
{
    /// <summary>
    /// 租户配置提供程序
    /// </summary>
    protected ITenantConfigurationProvider TenantConfigurationProvider { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    /// <param name="signInManager"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="tenantConfigurationProvider"></param>
    /// <param name="currentTenant"></param>
    public StarshineSecurityStampValidator(
        IOptions<SecurityStampValidatorOptions> options,
        SignInManager<IdentityUser> signInManager,
        ILoggerFactory loggerFactory,
        ITenantConfigurationProvider tenantConfigurationProvider,
        ICurrentTenant currentTenant)
        : base(
            options,
            signInManager,
            loggerFactory)
    {
        TenantConfigurationProvider = tenantConfigurationProvider;
        CurrentTenant = currentTenant;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    [UnitOfWork]
    public async override Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        TenantConfiguration? tenant = null;
        try
        {
            tenant = await TenantConfigurationProvider.GetAsync(saveResolveResult: false);
        }
        catch (Exception e)
        {
            Logger.LogException(e);
        }

        using (CurrentTenant.Change(tenant?.Id, tenant?.Name))
        {
            await base.ValidateAsync(context);
        }
    }
}
