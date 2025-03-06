using System;
using System.Threading;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Domain.Repositories;

/// <summary>
/// 实体变更跟踪提供商。
/// </summary>
public class EntityChangeTrackingProvider : IEntityChangeTrackingProvider, ISingletonDependency
{
    /// <summary>
    /// 是否启用。
    /// </summary>
    public bool? Enabled => _current.Value;

    private readonly AsyncLocal<bool?> _current = new AsyncLocal<bool?>();

    /// <summary>
    /// 变更跟踪。
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    public IDisposable Change(bool? enabled)
    {
        var previousValue = Enabled;
        _current.Value = enabled;
        return new DisposeAction(() => _current.Value = previousValue);
    }
}
