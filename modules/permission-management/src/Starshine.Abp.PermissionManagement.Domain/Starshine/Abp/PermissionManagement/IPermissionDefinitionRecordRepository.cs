using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.PermissionManagement;

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