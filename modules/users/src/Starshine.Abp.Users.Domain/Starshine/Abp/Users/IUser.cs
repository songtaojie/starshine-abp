using JetBrains.Annotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Users;

/// <summary>
/// 
/// </summary>
public interface IUser : IAggregateRoot<Guid>, IMultiTenant, IHasExtraProperties
{
    /// <summary>
    /// �û���
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// ����
    /// </summary>
    [CanBeNull]
    string? Email { get; }

    /// <summary>
    /// �û�����
    /// </summary>
    [CanBeNull]
    string? Name { get; }

    /// <summary>
    /// �û���
    /// </summary>
    [CanBeNull]
    string? Surname { get; }

    /// <summary>
    /// �Ƿ񼤻�
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// �����Ƿ�ȷ��
    /// </summary>
    bool EmailConfirmed { get; }

    /// <summary>
    /// �ֻ�����
    /// </summary>
    [CanBeNull]
    string? PhoneNumber { get; }

    /// <summary>
    /// �ֻ����Ƿ�ȷ��
    /// </summary>
    bool PhoneNumberConfirmed { get; }
}
