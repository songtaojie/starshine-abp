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
/// �������ࡣ
/// </summary>
/// <remarks>
/// ���캯����
/// </remarks>
/// <param name="lazyServiceProvider"></param>
public abstract class DomainService(IAbpLazyServiceProvider lazyServiceProvider) : IDomainService
{

    /// <summary>
    /// ����ķ����ṩ������ȡ����
    /// </summary>
    protected IAbpLazyServiceProvider LazyServiceProvider { get; } = lazyServiceProvider;

    /// <summary>
    /// ʱ�ӷ���
    /// </summary>
    protected IClock Clock => LazyServiceProvider.LazyGetRequiredService<IClock>();

    /// <summary>
    /// Guid��������
    /// </summary>
    protected IGuidGenerator GuidGenerator => LazyServiceProvider.LazyGetService<IGuidGenerator>(SimpleGuidGenerator.Instance);

    /// <summary>
    /// ��־������
    /// </summary>
    private ILoggerFactory LoggerFactory => LazyServiceProvider.LazyGetRequiredService<ILoggerFactory>();

    /// <summary>
    /// ��ǰ�⻧��
    /// </summary>
    protected ICurrentTenant CurrentTenant => LazyServiceProvider.LazyGetRequiredService<ICurrentTenant>();

    /// <summary>
    /// �첽��ѯִ������
    /// </summary>
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    /// <summary>
    /// ��־��
    /// </summary>
    protected ILogger Logger => LazyServiceProvider.LazyGetService<ILogger>(provider => LoggerFactory?.CreateLogger(GetType().FullName!) ?? NullLogger.Instance);
}
