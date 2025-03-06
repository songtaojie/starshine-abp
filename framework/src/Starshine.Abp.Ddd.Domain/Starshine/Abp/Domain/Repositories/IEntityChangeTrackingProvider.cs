using System;

namespace Starshine.Abp.Domain.Repositories;
/// <summary>
/// 实体变更跟踪提供程序
/// </summary>
public interface IEntityChangeTrackingProvider
{
    /// <summary>
    /// 是否启用
    /// </summary>
    bool? Enabled { get; }

    /// <summary>
    /// 启用/禁用
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    IDisposable Change(bool? enabled);
}
