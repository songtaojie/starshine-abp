using Starshine.Abp.TenantManagement.Localization;
using Starshine.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 应用服务基类
/// </summary>
public abstract class TenantManagementAppServiceBase : ApplicationService
{
    /// <summary>
    /// 应用服务基类
    /// </summary>
    protected TenantManagementAppServiceBase(IAbpLazyServiceProvider abpLazyServiceProvider)
        :base(abpLazyServiceProvider)
    {
        LocalizationResource = typeof(StarshineTenantManagementResource);
    }
}
