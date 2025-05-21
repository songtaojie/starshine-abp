using Starshine.Abp.Domain.Repositories;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限授予仓储
/// </summary>
public interface IPermissionGrantRepository : IBasicRepository<PermissionGrant, Guid>
{
    /// <summary>
    /// 根据名称获取
    /// </summary>
    /// <param name="name"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PermissionGrant?> FindAsync(string name,string providerName,string providerKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取所有授权
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PermissionGrant>> GetListAsync(string providerName,string providerKey,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取指定名称权限集合
    /// </summary>
    /// <param name="names"></param>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PermissionGrant>> GetListAsync(string[] names, string providerName, string providerKey,CancellationToken cancellationToken = default);
}
