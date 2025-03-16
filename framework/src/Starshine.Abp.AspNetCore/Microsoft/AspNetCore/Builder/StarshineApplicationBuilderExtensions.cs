using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.Options;
using Starshine.Abp.AspNetCore.Cors;
using Starshine.Abp.AspNetCore.ExceptionHandling;
using Starshine.Abp.AspNetCore.Tracing;
using Starshine.Abp.AspNetCore.Uow;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 跨域中间件拓展
    /// </summary>
    public static class StarshineApplicationBuilderExtensions
    {
        private const string ExceptionHandlingMiddlewareMarker = "_ExceptionHandlingMiddleware_Added";

        public async static Task InitializeApplicationAsync(this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                AsyncHelper.RunSync(() => application.ShutdownAsync());
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            await application.InitializeAsync(app.ApplicationServices);
        }

        public static void InitializeApplication(this IApplicationBuilder app)
        {
            Check.NotNull(app, nameof(app));

            app.ApplicationServices.GetRequiredService<ObjectAccessor<IApplicationBuilder>>().Value = app;
            var application = app.ApplicationServices.GetRequiredService<IAbpApplicationWithExternalServiceProvider>();
            var applicationLifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();

            applicationLifetime.ApplicationStopping.Register(() =>
            {
                application.Shutdown();
            });

            applicationLifetime.ApplicationStopped.Register(() =>
            {
                application.Dispose();
            });

            application.Initialize(app.ApplicationServices);
        }




        public static IApplicationBuilder UseUnitOfWork(this IApplicationBuilder app)
        {
            return app
                .UseAbpExceptionHandling()
                .UseMiddleware<UnitOfWorkMiddleware>();
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        {
            return app
                .UseMiddleware<CorrelationIdMiddleware>();
        }

        public static IApplicationBuilder UseStarshineRequestLocalization(this IApplicationBuilder app,
            Action<RequestLocalizationOptions>? optionsAction = null)
        {
            app.ApplicationServices
                .GetRequiredService<IRequestLocalizationOptionsProvider>()
                .InitLocalizationOptions(optionsAction);

            return app.UseMiddleware<RequestLocalizationMiddleware>();
        }

        public static IApplicationBuilder UseAbpExceptionHandling(this IApplicationBuilder app)
        {
            if (app.Properties.ContainsKey(ExceptionHandlingMiddlewareMarker))
            {
                return app;
            }

            app.Properties[ExceptionHandlingMiddlewareMarker] = true;
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }


        /// <summary>
        /// 添加跨域中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorsAccessor(this IApplicationBuilder app)
        {
            // 获取选项
            var options = app.ApplicationServices.GetService<IOptions<CorsSettingsOptions>>();
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Add the AddCorsAccessor method to services");
            var corsAccessorSettings = options.Value;
            if (corsAccessorSettings.EnabledSignalR ?? false)
            {
                // 配置跨域中间件
                app.UseCors(builder =>
                {
                    CorsAccessorPolicy.SetCorsPolicy(builder, corsAccessorSettings, true);
                });
            }
            else
            {
                // 配置跨域中间件
                app.UseCors(corsAccessorSettings.PolicyName!);
            }

            return app;
        }
    }
}