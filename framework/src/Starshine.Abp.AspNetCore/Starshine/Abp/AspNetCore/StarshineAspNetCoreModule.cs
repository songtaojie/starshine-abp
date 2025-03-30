using Microsoft.Extensions.FileProviders;
using Starshine.Abp.Core;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.AspNetCore
{
    /// <summary>
    /// StarshineAbpAspNetCore模块入口
    /// </summary>
    [DependsOn(
     typeof(AbpAspNetCoreModule)
     )]
    public class StarshineAspNetCoreModule : StarshineAbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStarshineCors();
        }


        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.ServiceProvider.GetService<IApplicationBuilder>();
            app?.UseStarshineCors();
        }
    }
}
