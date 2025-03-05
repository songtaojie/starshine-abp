using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;

/// <summary>
/// 标识链接用户存储库
/// </summary>
public interface IIdentityLinkUserRepository : IBasicRepository<IdentityLinkUser, Guid>
{
    /// <summary>
    /// 获取
    /// </summary>
    /// <param name="sourceLinkUserInfo"></param>
    /// <param name="targetLinkUserInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdentityLinkUser?> FindAsync(IdentityLinkUserInfo sourceLinkUserInfo,IdentityLinkUserInfo targetLinkUserInfo,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="linkUserInfo"></param>
    /// <param name="excludes"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, List<IdentityLinkUserInfo>? excludes = null,CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="linkUserInfo"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(IdentityLinkUserInfo linkUserInfo, CancellationToken cancellationToken = default);
}
