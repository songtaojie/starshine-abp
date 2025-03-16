using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.FileProviders;
using Starshine.Abp.AspNetCore.Auditing;
using Starshine.Abp.AspNetCore.VirtualFileSystem;
using Starshine.Abp.Core;
using Volo.Abp;
using Volo.Abp.AspNetCore;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
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
     typeof(AbpAuditingModule),
     typeof(AbpSecurityModule),
     typeof(AbpVirtualFileSystemModule),
     typeof(AbpUnitOfWorkModule),
     typeof(AbpAuthorizationModule),
     typeof(AbpValidationModule),
     typeof(AbpExceptionHandlingModule),
     typeof(AbpAspNetCoreAbstractionsModule)
     )]
    public class StarshineAspNetCoreModule : StarshineAbpModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization();

            Configure<AbpAuditingOptions,IServiceProvider>((options,privoder) =>
            {
                options.Contributors.Add(new AspNetCoreAuditLogContributor(privoder.GetRequiredService<ILogger<AspNetCoreAuditLogContributor>>()));
            });

            Configure<StaticFileOptions>(options =>
            {
                options.ContentTypeProvider = context.Services.GetRequiredService<StarshineFileExtensionContentTypeProvider>();
            });

            AddAspNetServices(context.Services);
            context.Services.AddObjectAccessor<IApplicationBuilder>();
            context.Services.AddAbpDynamicOptions<RequestLocalizationOptions, RequestLocalizationOptionsManager>();
        }

        private static void AddAspNetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var environment = context.ServiceProvider.GetService<IWebHostEnvironment>();
            if (environment != null)
            {
                environment.WebRootFileProvider =
                    new CompositeFileProvider(
                        environment.WebRootFileProvider,
                        context.ServiceProvider.GetRequiredService<IWebContentFileProvider>()
                    );
            }
        }
    }
}
