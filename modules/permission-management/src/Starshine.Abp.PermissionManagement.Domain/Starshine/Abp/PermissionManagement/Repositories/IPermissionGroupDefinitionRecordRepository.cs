using Starshine.Abp.Domain.Repositories;
using Starshine.Abp.PermissionManagement.Entities;

namespace Starshine.Abp.PermissionManagement.Repositories;

/// <summary>
/// 权限组定义记录存储库
/// </summary>
public interface IPermissionGroupDefinitionRecordRepository : IBasicRepository<PermissionGroupDefinitionRecord, Guid>
{

}