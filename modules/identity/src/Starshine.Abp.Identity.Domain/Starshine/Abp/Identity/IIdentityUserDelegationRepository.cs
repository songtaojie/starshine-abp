using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份用户委托存储库
/// </summary>
public interface IIdentityUserDelegationRepository : IBasicRepository<IdentityUserDelegation, Guid>
{
    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="targetUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId,Guid? targetUserId,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取可用的委托
    /// </summary>
    /// <param name="targetUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUserId,CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdentityUserDelegation?> FindActiveDelegationByIdAsync(Guid id,CancellationToken cancellationToken = default);
}
