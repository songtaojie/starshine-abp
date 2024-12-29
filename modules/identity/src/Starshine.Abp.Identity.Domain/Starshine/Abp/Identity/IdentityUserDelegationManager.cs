using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份用户委派管理器
/// </summary>
public class IdentityUserDelegationManager : DomainService
{
    /// <summary>
    /// 身份用户委托存储库
    /// </summary>
    protected IIdentityUserDelegationRepository IdentityUserDelegationRepository { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityUserDelegationRepository"></param>
    public IdentityUserDelegationManager(IIdentityUserDelegationRepository identityUserDelegationRepository)
    {
        IdentityUserDelegationRepository = identityUserDelegationRepository;
    }
    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="targetUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUserDelegation>> GetListAsync(Guid? sourceUserId = null, Guid? targetUserId = null, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetListAsync(sourceUserId, targetUserId, cancellationToken: cancellationToken);
    }
    /// <summary>
    /// 获取可用的尾货
    /// </summary>
    /// <param name="targetUseId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<List<IdentityUserDelegation>> GetActiveDelegationsAsync(Guid targetUseId, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.GetActiveDelegationsAsync(targetUseId, cancellationToken: cancellationToken);
    }
    /// <summary>
    /// 获取可用的委托
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUserDelegation> FindActiveDelegationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await IdentityUserDelegationRepository.FindActiveDelegationByIdAsync(id, cancellationToken: cancellationToken);
    }
    /// <summary>
    /// 委托用户
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="targetUserId"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    public virtual async Task DelegateNewUserAsync(Guid sourceUserId, Guid targetUserId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default)
    {
        if (sourceUserId == targetUserId)
        {
            throw new BusinessException(IdentityErrorCodes.YouCannotDelegateYourself);
        }

        await IdentityUserDelegationRepository.InsertAsync(new IdentityUserDelegation(
                GuidGenerator.Create(),
                sourceUserId,
                targetUserId,
                startTime,
                endTime,
                CurrentTenant.Id
            ),
            cancellationToken: cancellationToken
        );
    }
    /// <summary>
    /// 删除委托数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sourceUserId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task DeleteDelegationAsync(Guid id, Guid sourceUserId, CancellationToken cancellationToken = default)
    {
        var delegation = await IdentityUserDelegationRepository.FindAsync(id, cancellationToken: cancellationToken);

        if (delegation != null && delegation.SourceUserId == sourceUserId)
        {
            await IdentityUserDelegationRepository.DeleteAsync(delegation, cancellationToken: cancellationToken);
        }
    }
}
