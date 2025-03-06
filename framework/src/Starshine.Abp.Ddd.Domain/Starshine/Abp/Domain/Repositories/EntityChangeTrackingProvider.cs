using System;
using System.Threading;
using Volo.Abp;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Domain.Repositories;

/// <summary>
/// ʵ���������ṩ�̡�
/// </summary>
public class EntityChangeTrackingProvider : IEntityChangeTrackingProvider, ISingletonDependency
{
    /// <summary>
    /// �Ƿ����á�
    /// </summary>
    public bool? Enabled => _current.Value;

    private readonly AsyncLocal<bool?> _current = new AsyncLocal<bool?>();

    /// <summary>
    /// ������١�
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
