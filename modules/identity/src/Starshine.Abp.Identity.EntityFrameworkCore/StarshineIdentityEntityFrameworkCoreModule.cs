using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.Users.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Identity.EntityFrameworkCore;

/// <summary>
/// Starshine EntityFrameworkCore模块
/// </summary>
[DependsOn(
    typeof(StarshineIdentityDomainModule),
    typeof(StarshineUsersEntityFrameworkCoreModule))]
public class StarshineAbpIdentityEntityFrameworkCoreModule : AbpModule
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<IdentityDbContext>(options =>
        {
            options.AddRepository<IdentityUser, EfCoreIdentityUserRepository>();
            options.AddRepository<IdentityRole, EfCoreIdentityRoleRepository>();
            options.AddRepository<IdentityClaimType, EfCoreIdentityClaimTypeRepository>();
            options.AddRepository<OrganizationUnit, EfCoreOrganizationUnitRepository>();
            options.AddRepository<IdentitySecurityLog, EfCoreIdentitySecurityLogRepository>();
            options.AddRepository<IdentityLinkUser, EfCoreIdentityLinkUserRepository>();
            options.AddRepository<IdentityUserDelegation, EfCoreIdentityUserDelegationRepository>();
            options.AddRepository<IdentitySession, EfCoreIdentitySessionRepository>();
        });
    }
}
