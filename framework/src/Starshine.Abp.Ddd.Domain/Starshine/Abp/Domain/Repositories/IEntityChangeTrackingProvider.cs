using System;

namespace Starshine.Abp.Domain.Repositories;
/// <summary>
/// ʵ���������ṩ����
/// </summary>
public interface IEntityChangeTrackingProvider
{
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    bool? Enabled { get; }

    /// <summary>
    /// ����/����
    /// </summary>
    /// <param name="enabled"></param>
    /// <returns></returns>
    IDisposable Change(bool? enabled);
}
