using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Guids;
using Starshine.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Starshine.Abp.PermissionManagement;

namespace Volo.Abp.PermissionManagement.Identity;

/// <summary>
/// 角色权限管理提供者
/// </summary>
public class RolePermissionManagementProvider : PermissionManagementProvider
{
    /// <summary>
    /// 提供者名称
    /// </summary>
    public override string Name => RolePermissionValueProvider.ProviderName;

    /// <summary>
    /// 用户角色查找器
    /// </summary>
    protected IUserRoleFinder UserRoleFinder { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permissionGrantRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="currentTenant"></param>
    /// <param name="userRoleFinder"></param>
    public RolePermissionManagementProvider(
        IPermissionGrantRepository permissionGrantRepository,
        IGuidGenerator guidGenerator,
        ICurrentTenant currentTenant,
        IUserRoleFinder userRoleFinder)
        : base(
            permissionGrantRepository,
            guidGenerator,
            currentTenant)
    {
        UserRoleFinder = userRoleFinder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public async override Task<PermissionValueProviderGrantInfo> CheckAsync(string name, string providerName, string providerKey)
    {
        var multipleGrantInfo = await CheckAsync(new[] { name }, providerName, providerKey);

        return multipleGrantInfo.Result.Values.First();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public async override Task<MultiplePermissionValueProviderGrantInfo> CheckAsync(string[] names, string providerName, string providerKey)
    {
        var multiplePermissionValueProviderGrantInfo = new MultiplePermissionValueProviderGrantInfo(names);
        var permissionGrants = new List<PermissionGrant>();

        if (providerName == Name)
        {
            permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, providerName, providerKey));

        }

        if (providerName == UserPermissionValueProvider.ProviderName)
        {
            var userId = Guid.Parse(providerKey);
            var roleNames = await UserRoleFinder.GetRoleNamesAsync(userId);

            foreach (var roleName in roleNames)
            {
                permissionGrants.AddRange(await PermissionGrantRepository.GetListAsync(names, Name, roleName));
            }
        }

        permissionGrants = permissionGrants.Distinct().ToList();
        if (!permissionGrants.Any())
        {
            return multiplePermissionValueProviderGrantInfo;
        }

        foreach (var permissionName in names)
        {
            var permissionGrant = permissionGrants.FirstOrDefault(x => x.Name == permissionName);
            if (permissionGrant != null)
            {
                multiplePermissionValueProviderGrantInfo.Result[permissionName] = new PermissionValueProviderGrantInfo(true, permissionGrant.ProviderKey);
            }
        }

        return multiplePermissionValueProviderGrantInfo;
    }
}
