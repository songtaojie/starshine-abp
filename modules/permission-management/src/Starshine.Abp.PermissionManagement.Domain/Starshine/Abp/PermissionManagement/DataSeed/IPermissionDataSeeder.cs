using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Starshine.Abp.PermissionManagement;
/// <summary>
/// 权限种子数据
/// </summary>
public interface IPermissionDataSeeder
{
    /// <summary>
    /// 种子初始化
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="grantedPermissions"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task SeedAsync(string providerName,string providerKey,IEnumerable<string> grantedPermissions,Guid? tenantId = null);
}
