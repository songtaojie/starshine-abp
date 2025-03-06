using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Linq;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace Starshine.Abp.Domain.Services;
/// <summary>
/// 域服务基类。
/// </summary>
/// <remarks>
/// 构造函数。
/// </remarks>
/// <param name="lazyServiceProvider"></param>
public abstract class DomainService(IAbpLazyServiceProvider lazyServiceProvider) : IDomainService
{

    /// <summary>
    /// 懒惰的服务提供者来获取服务。
    /// </summary>
    protected IAbpLazyServiceProvider LazyServiceProvider { get; } = lazyServiceProvider;

    /// <summary>
    /// 时钟服务。
    /// </summary>
    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    /// <summary>
    /// Guid生成器。
    /// </summary>
    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    /// <summary>
    /// 日志工厂。
    /// </summary>
    private ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    /// <summary>
    /// 当前租户。
    /// </summary>
    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    /// <summary>
    /// 异步查询执行器。
    /// </summary>
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    /// <summary>
    /// 日志。
    /// </summary>
    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);
}
