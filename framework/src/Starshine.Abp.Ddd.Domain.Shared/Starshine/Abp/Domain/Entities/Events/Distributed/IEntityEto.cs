namespace Starshine.Abp.Domain.Entities.Events.Distributed;

/// <summary>
/// ʵ������¼�����
/// </summary>
/// <typeparam name="TKey"></typeparam>
public interface IEntityEto<TKey>
{
    /// <summary>
    /// ��ʵ���Ψһ��ʶ����
    /// </summary>
    TKey Id { get; set; }
}