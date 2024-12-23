using System;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.PermissionManagement;

public interface IPermissionGroupDefinitionRecordRepository : IBasicRepository<PermissionGroupDefinitionRecord, Guid>
{

}