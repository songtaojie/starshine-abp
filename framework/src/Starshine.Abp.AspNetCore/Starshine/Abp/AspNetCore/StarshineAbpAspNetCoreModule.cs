using Starshine.Abp.Core;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.AspNetCore
{
    [DependsOn(
    typeof(AbpSecurityModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpUnitOfWorkModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpValidationModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpAspNetCoreAbstractionsModule)
    )]
    public class StarshineAbpAspNetCoreModule : StarshineAbpModule
    {

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseStaticFiles();
            app.UseAbpSecurityHeaders();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAuditing();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
        }
    }
}
