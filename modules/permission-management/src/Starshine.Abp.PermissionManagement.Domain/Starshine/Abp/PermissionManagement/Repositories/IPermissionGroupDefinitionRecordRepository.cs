using System;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限组定义记录存储库
/// </summary>
public interface IPermissionGroupDefinitionRecordRepository : IBasicRepository<PermissionGroupDefinitionRecord, Guid>
{

}