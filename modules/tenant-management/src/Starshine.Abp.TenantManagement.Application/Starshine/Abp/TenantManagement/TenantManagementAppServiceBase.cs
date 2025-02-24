using Volo.Abp.Application.Services;
using Starshine.Abp.TenantManagement.Localization;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 应用服务基类
/// </summary>
public abstract class TenantManagementAppServiceBase : ApplicationService
{
    /// <summary>
    /// 应用服务基类
    /// </summary>
    protected TenantManagementAppServiceBase()
    {
        LocalizationResource = typeof(StarshineTenantManagementResource);
    }
}
