using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.ObjectExtending;
using Starshine.Abp.Users;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Threading;

namespace Starshine.Abp.Identity;

/// <summary>
/// StarshineAbp 身份域模块
/// </summary>
[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(StarshineAbpIdentityDomainSharedModule),
    typeof(StarshineAbpUsersDomainModule),
    typeof(AbpAutoMapperModule)
    )]
public class StarshineIdentityDomainModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    /// <summary>
    /// 前置配置
    /// </summary>
    /// <param name="context"></param>
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsRemoteRefreshEnabled = false;
        });
    }

    /// <summary>
    /// 服务配置
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<StarshineIdentityDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<IdentityDomainMappingProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<IdentityUser, UserEto>(typeof(StarshineIdentityDomainModule));
            options.EtoMappings.Add<IdentityClaimType, IdentityClaimTypeEto>(typeof(StarshineIdentityDomainModule));
            options.EtoMappings.Add<IdentityRole, IdentityRoleEto>(typeof(StarshineIdentityDomainModule));
            options.EtoMappings.Add<OrganizationUnit, OrganizationUnitEto>(typeof(StarshineIdentityDomainModule));

            options.AutoEventSelectors.Add<IdentityUser>();
            options.AutoEventSelectors.Add<IdentityRole>();
        });

        var identityBuilder = context.Services.AddStarshineIdentity(options =>
        {
            options.User.RequireUniqueEmail = true;
        });

        context.Services.AddObjectAccessor(identityBuilder);
        context.Services.ExecutePreConfiguredActions(identityBuilder);

        Configure<IdentityOptions>(options =>
        {
            options.ClaimsIdentity.UserIdClaimType = AbpClaimTypes.UserId;
            options.ClaimsIdentity.UserNameClaimType = AbpClaimTypes.UserName;
            options.ClaimsIdentity.RoleClaimType = AbpClaimTypes.Role;
            options.ClaimsIdentity.EmailClaimType = AbpClaimTypes.Email;
        });

        context.Services.AddAbpDynamicOptions<IdentityOptions, StarshineIdentityOptionsManager>();
    }

    /// <summary>
    /// 后置配置
    /// </summary>
    /// <param name="context"></param>
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.User,
                typeof(IdentityUser)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.Role,
                typeof(IdentityRole)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.ClaimType,
                typeof(IdentityClaimType)
            );

            ModuleExtensionConfigurationHelper.ApplyEntityConfigurationToEntity(
                IdentityModuleExtensionConsts.ModuleName,
                IdentityModuleExtensionConsts.EntityNames.OrganizationUnit,
                typeof(OrganizationUnit)
            );
        });
    }
}
