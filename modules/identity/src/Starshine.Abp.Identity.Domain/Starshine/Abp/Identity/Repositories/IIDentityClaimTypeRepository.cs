using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份声明类型存储库
/// </summary>
public interface IIdentityClaimTypeRepository : IBasicRepository<IdentityClaimType, Guid>
{
    /// <summary>
    /// 检查是否存在具有给定名称的 <see cref="IdentityClaimType"/> 实体。
    /// </summary>
    /// <param name="name">要检查的名称</param>
    /// <param name="ignoredId">
    /// 检查时要忽略的 ID 值。如果存在具有给定 <paramref name="ignoredId"/> 的实体，则会将其忽略。
    /// </param>
    /// <param name="cancellationToken">Cancel token</param>
    Task<bool> AnyAsync(string name,Guid? ignoredId = null,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取符合条件的列表数据
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityClaimType>> GetListAsync(string sorting,int maxResultCount,int skipCount,string? filter = null,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取符合条件的数量
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(string? filter = null,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取给定名字的列表数据
    /// </summary>
    /// <param name="names"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityClaimType>> GetListByNamesAsync(IEnumerable<string> names,CancellationToken cancellationToken = default);
}
