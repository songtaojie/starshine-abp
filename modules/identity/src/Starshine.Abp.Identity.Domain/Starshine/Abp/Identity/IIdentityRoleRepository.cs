using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份角色存储库
/// </summary>
public interface IIdentityRoleRepository : IBasicRepository<IdentityRole, Guid>
{
    /// <summary>
    /// 通过异步操作查找具有指定规范化名称的角色。
    /// </summary>
    /// <param name="normalizedRoleName">要查找的规范化角色名称。</param>
    /// <param name="includeDetails">是否包含明细</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 查找的结果。</returns>
    Task<IdentityRole?> FindByNormalizedNameAsync(string normalizedRoleName,bool includeDetails = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRoleWithUserCount>> GetListWithUserCountAsync(string? sorting = null, int maxResultCount = int.MaxValue,int skipCount = 0,string? filter = null,bool includeDetails = false,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetListAsync(IEnumerable<Guid> ids,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取默认角色数据
    /// </summary>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetDefaultOnesAsync(bool includeDetails = false,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取符合条件的数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? filter = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="claimType"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveClaimFromAllRolesAsync(string claimType,bool autoSave = false,CancellationToken cancellationToken = default);
}
