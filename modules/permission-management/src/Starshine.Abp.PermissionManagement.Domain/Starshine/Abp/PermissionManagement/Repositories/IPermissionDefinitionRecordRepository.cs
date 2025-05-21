using Starshine.Abp.Domain.Repositories;
using Starshine.Abp.PermissionManagement.Entities;

namespace Starshine.Abp.PermissionManagement.Repositories;

/// <summary>
/// Ȩ�޶����¼�ִ�
/// </summary>
public interface IPermissionDefinitionRecordRepository : IBasicRepository<PermissionDefinitionRecord, Guid>
{
    /// <summary>
    /// �������ֻ�ȡȨ�޶����¼
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PermissionDefinitionRecord?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}