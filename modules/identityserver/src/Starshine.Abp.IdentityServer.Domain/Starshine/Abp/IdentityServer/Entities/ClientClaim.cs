using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.IdentityServer.Entities;
/// <summary>
/// �ͻ�������
/// </summary>
public class ClientClaim : Entity
{
    /// <summary>
    /// �ͻ���Id
    /// </summary>
    public required virtual Guid ClientId { get; set; }

    /// <summary>
    /// �ͻ�����������
    /// </summary>
    public required virtual string Type { get; set; }

    /// <summary>
    /// �ͻ�������ֵ
    /// </summary>
    public required virtual string Value { get; set; }

    /// <summary>
    /// ���캯��
    /// </summary>
    protected internal ClientClaim()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool Equals(Guid clientId, string type, string value)
    {
        return ClientId == clientId && Type == type && Value == value;
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <returns></returns>
    public override object[] GetKeys()
    {
        return [ClientId, Type, Value];
    }
}
