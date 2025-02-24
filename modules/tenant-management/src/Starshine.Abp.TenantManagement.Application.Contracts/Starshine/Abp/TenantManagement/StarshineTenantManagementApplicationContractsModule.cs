using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectExtending.Modularity;
using Volo.Abp.Threading;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户管理应用合同模块
/// </summary>
[DependsOn(
    typeof(AbpDddApplicationContractsModule),
    typeof(StarshineTenantManagementDomainSharedModule),
    typeof(AbpAuthorizationAbstractionsModule)
    )]
public class StarshineTenantManagementApplicationContractsModule : AbpModule
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    /// <summary>
    /// 配置服务后
    /// </summary>
    /// <param name="context"></param>
    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        OneTimeRunner.Run(() =>
        {
            ModuleExtensionConfigurationHelper
                .ApplyEntityConfigurationToApi(
                    TenantManagementModuleExtensionConsts.ModuleName,
                    TenantManagementModuleExtensionConsts.EntityNames.Tenant,
                    getApiTypes: [typeof(TenantDto)],
                    createApiTypes: [typeof(TenantCreateDto)],
                    updateApiTypes: [typeof(TenantUpdateDto)]
                );
        });
    }
}
