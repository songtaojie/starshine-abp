using Starshine.Abp.Domain.Repositories;
using Starshine.Abp.PermissionManagement.Entities;

namespace Starshine.Abp.PermissionManagement.Repositories;

/// <summary>
/// 权限定义记录仓储
/// </summary>
public interface IPermissionDefinitionRecordRepository : IBasicRepository<PermissionDefinitionRecord, Guid>
{
    /// <summary>
    /// 根据名字获取权限定义记录
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PermissionDefinitionRecord?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
}