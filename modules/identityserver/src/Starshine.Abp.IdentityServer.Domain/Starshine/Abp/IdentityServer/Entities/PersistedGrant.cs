using System;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// �־û�����
/// </summary>
public class PersistedGrant : AggregateRoot<Guid>
{
    /// <summary>
    /// ��
    /// </summary>
    public required virtual string Key { get; set; }

    /// <summary>
    /// ����  
    /// </summary>
    public required virtual string Type { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public virtual string? SubjectId { get; set; }

    /// <summary>
    /// �Ự
    /// </summary>
    public virtual string? SessionId { get; set; }

    /// <summary>
    /// �ͻ���
    /// </summary>
    public required virtual string ClientId { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual DateTimeOffset CreationTime { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual DateTimeOffset? Expiration { get; set; }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    public virtual DateTimeOffset? ConsumedTime { get; set; }

    /// <summary>
    /// ����
    /// </summary>
    public virtual string? Data { get; set; }

    /// <summary>
    /// Ĭ�Ϲ��캯��
    /// </summary>
    protected internal PersistedGrant()
    {
    }

    /// <summary>
    /// ���캯��
    /// </summary>
    /// <param name="id"></param>
    public PersistedGrant(Guid id)
        : base(id)
    {
    }
}
