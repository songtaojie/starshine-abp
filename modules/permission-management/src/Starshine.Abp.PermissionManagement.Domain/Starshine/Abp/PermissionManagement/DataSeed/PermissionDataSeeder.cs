using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.PermissionManagement;
/// <summary>
/// 权限种子数据
/// </summary>
public class PermissionDataSeeder : IPermissionDataSeeder, ITransientDependency
{
    /// <summary>
    /// 权限仓储
    /// </summary>
    protected IPermissionGrantRepository PermissionGrantRepository { get; }
    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }

    /// <summary>
    /// 当前租户
    /// </summary>
    protected ICurrentTenant CurrentTenant { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    public PermissionDataSeeder(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant)
    {
        PermissionGrantRepository = permissionGrantRepository;
        GuidGenerator = guidGenerator;
        CurrentTenant = currentTenant;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="grantedPermissions"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public virtual async Task SeedAsync(string providerName,string providerKey,IEnumerable<string> grantedPermissions,Guid? tenantId = null)
    {
        using (CurrentTenant.Change(tenantId))
        {
            var names = grantedPermissions.ToArray();
            var existsPermissionGrants = (await PermissionGrantRepository.GetListAsync(names, providerName, providerKey)).Select(x => x.Name).ToList();

            foreach (var permissionName in names.Except(existsPermissionGrants))
            {
                await PermissionGrantRepository.InsertAsync(
                    new PermissionGrant(
                        GuidGenerator.Create(),
                        permissionName,
                        providerName,
                        providerKey,
                        tenantId
                    )
                );
            }
        }
    }
}
