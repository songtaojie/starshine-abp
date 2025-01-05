using Volo.Abp.Application.Services;
using Starshine.Abp.Identity.Localization;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public abstract class IdentityAppServiceBase : ApplicationService
{
    /// <summary>
    /// 
    /// </summary>
    protected IdentityAppServiceBase(IAbpLazyServiceProvider abpLazyServiceProvider)
    {
        ObjectMapperContext = typeof(StarshineIdentityApplicationModule);
        LocalizationResource = typeof(IdentityResource);
        LazyServiceProvider = abpLazyServiceProvider;
    }
}
