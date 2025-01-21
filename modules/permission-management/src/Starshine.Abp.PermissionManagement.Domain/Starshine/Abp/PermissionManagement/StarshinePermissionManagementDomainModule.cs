using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Volo.Abp;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限管理领域模型
/// </summary>
[DependsOn(typeof(AbpAuthorizationModule))]
[DependsOn(typeof(AbpDddDomainModule))]
[DependsOn(typeof(StarshinePermissionManagementDomainSharedModule))]
[DependsOn(typeof(AbpCachingModule))]
[DependsOn(typeof(AbpJsonModule))]
public class StarshinePermissionManagementDomainModule : AbpModule
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Task? _initializeDynamicPermissionsTask;
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="context"></param>
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        if (context.Services.IsDataMigrationEnvironment())
        {
            Configure<PermissionManagementOptions>(options =>
            {
                options.SaveStaticPermissionsToDatabase = false;
                options.IsDynamicPermissionStoreEnabled = false;
            });
        }
    }

    /// <summary>
    /// 应用启动
    /// </summary>
    /// <param name="context"></param>
    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
    }

    /// <summary>
    /// 应用启动
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        InitializeDynamicPermissions(context);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 应用关闭时
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override Task OnApplicationShutdownAsync(ApplicationShutdownContext context)
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 获取动态权限
    /// </summary>
    /// <returns></returns>
    public Task GetInitializeDynamicPermissionsTask()
    {
        return _initializeDynamicPermissionsTask ?? Task.CompletedTask;
    }

    /// <summary>
    /// 初始化权限数据
    /// </summary>
    /// <param name="context"></param>
    private void InitializeDynamicPermissions(ApplicationInitializationContext context)
    {
        var options = context.ServiceProvider.GetRequiredService<IOptions<PermissionManagementOptions>>().Value;

        if (!options.SaveStaticPermissionsToDatabase && !options.IsDynamicPermissionStoreEnabled)
        {
            return;
        }

        var rootServiceProvider = context.ServiceProvider.GetRequiredService<IRootServiceProvider>();

        _initializeDynamicPermissionsTask = Task.Run(async () =>
        {
            using var scope = rootServiceProvider.CreateScope();
            var applicationLifetime = scope.ServiceProvider.GetService<IHostApplicationLifetime>();
            var cancellationTokenProvider = scope.ServiceProvider.GetRequiredService<ICancellationTokenProvider>();
            var cancellationToken = applicationLifetime?.ApplicationStopping ?? _cancellationTokenSource.Token;

            try
            {
                using (cancellationTokenProvider.Use(cancellationToken))
                {
                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await SaveStaticPermissionsToDatabaseAsync(options, scope, cancellationTokenProvider);

                    if (cancellationTokenProvider.Token.IsCancellationRequested)
                    {
                        return;
                    }

                    await PreCacheDynamicPermissionsAsync(options, scope);
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause (No need to log since it is logged above)
            catch { }
        });
    }

    private async static Task SaveStaticPermissionsToDatabaseAsync(PermissionManagementOptions options, IServiceScope scope,ICancellationTokenProvider cancellationTokenProvider)
    {
        if (!options.SaveStaticPermissionsToDatabase)
        {
            return;
        }

        await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                8,
                retryAttempt => TimeSpan.FromSeconds(RandomHelper.GetRandom((int)Math.Pow(2, retryAttempt) * 8,(int)Math.Pow(2, retryAttempt) * 12) )
            )
            .ExecuteAsync(async _ =>
            {
                try
                {
                    await scope.ServiceProvider.GetRequiredService<IStaticPermissionSaver>()
                        .SaveAsync();
                }
                catch (Exception ex)
                {
                    scope.ServiceProvider
                        .GetService<ILogger<StarshinePermissionManagementDomainModule>>()?
                        .LogException(ex);

                    throw; 
                }
            }, cancellationTokenProvider.Token);
    }

    private async static Task PreCacheDynamicPermissionsAsync(PermissionManagementOptions options, IServiceScope scope)
    {
        if (!options.IsDynamicPermissionStoreEnabled)
        {
            return;
        }

        try
        {
            // 预先缓存权限，因此第一个请求无需等待
            await scope.ServiceProvider
                .GetRequiredService<IDynamicPermissionDefinitionStore>()
                .GetGroupsAsync();
        }
        catch (Exception ex)
        {
            // ReSharper disable once AccessToDisposedClosure
            scope.ServiceProvider
                .GetService<ILogger<StarshinePermissionManagementDomainModule>>()?
                .LogException(ex);

            throw; // It will be cached in InitializeDynamicPermissions
        }
    }
}
