using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
public class IdentityLinkUserManager : DomainService
{
    /// <summary>
    /// 
    /// </summary>
    protected IIdentityLinkUserRepository IdentityLinkUserRepository { get; }
    /// <summary>
    /// 用户管理
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// 当前租户
    /// </summary>
    protected new ICurrentTenant CurrentTenant { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityLinkUserRepository"></param>
    /// <param name="userManager"></param>
    /// <param name="currentTenant"></param>
    public IdentityLinkUserManager(IIdentityLinkUserRepository identityLinkUserRepository, IdentityUserManager userManager, ICurrentTenant currentTenant)
    {
        IdentityLinkUserRepository = identityLinkUserRepository;
        UserManager = userManager;
        CurrentTenant = currentTenant;
    }
    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="linkUserInfo"></param>
    /// <param name="includeIndirect"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<IdentityLinkUser>> GetListAsync(IdentityLinkUserInfo linkUserInfo, bool includeIndirect = false, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            var users = await IdentityLinkUserRepository.GetListAsync(linkUserInfo, cancellationToken: cancellationToken);
            if (includeIndirect == false)
            {
                return users;
            }

            var userInfos = new List<IdentityLinkUserInfo>()
                {
                    linkUserInfo
                };

            var allUsers = new List<IdentityLinkUser>();
            allUsers.AddRange(users);

            do
            {
                var nextUsers = new List<IdentityLinkUserInfo>();
                foreach (var user in users)
                {
                    if (userInfos.Any(x => x.TenantId != user.SourceTenantId || x.UserId != user.SourceUserId))
                    {
                        nextUsers.Add(new IdentityLinkUserInfo(user.SourceUserId, user.SourceTenantId));
                    }

                    if (userInfos.Any(x => x.TenantId != user.TargetTenantId || x.UserId != user.TargetUserId))
                    {
                        nextUsers.Add(new IdentityLinkUserInfo(user.TargetUserId, user.TargetTenantId));
                    }
                }

                users = new List<IdentityLinkUser>();
                foreach (var next in nextUsers)
                {
                    users.AddRange(await IdentityLinkUserRepository.GetListAsync(next, userInfos, cancellationToken));
                }

                userInfos.AddRange(nextUsers);
                allUsers.AddRange(users);
            } while (users.Count != 0);

            return allUsers;
        }
    }
    /// <summary>
    /// 关联用户
    /// </summary>
    /// <param name="sourceLinkUser"></param>
    /// <param name="targetLinkUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task LinkAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            if (sourceLinkUser.UserId == targetLinkUser.UserId && sourceLinkUser.TenantId == targetLinkUser.TenantId)
            {
                return;
            }

            if (await IsLinkedAsync(sourceLinkUser, targetLinkUser, cancellationToken: cancellationToken))
            {
                return;
            }

            var userLink = new IdentityLinkUser(GuidGenerator.Create(),sourceLinkUser,targetLinkUser);
            await IdentityLinkUserRepository.InsertAsync(userLink, true, cancellationToken);
        }
    }
    /// <summary>
    /// 是否关联
    /// </summary>
    /// <param name="sourceLinkUser"></param>
    /// <param name="targetLinkUser"></param>
    /// <param name="includeIndirect"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> IsLinkedAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser, bool includeIndirect = false, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            if (includeIndirect)
            {
                return (await GetListAsync(sourceLinkUser, true, cancellationToken: cancellationToken))
                    .Any(x => x.SourceTenantId == targetLinkUser.TenantId && x.SourceUserId == targetLinkUser.UserId ||
                              x.TargetTenantId == targetLinkUser.TenantId && x.TargetUserId == targetLinkUser.UserId);
            }
            return await IdentityLinkUserRepository.FindAsync(sourceLinkUser, targetLinkUser, cancellationToken) != null;
        }
    }
    /// <summary>
    /// 取消关联
    /// </summary>
    /// <param name="sourceLinkUser"></param>
    /// <param name="targetLinkUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task UnlinkAsync(IdentityLinkUserInfo sourceLinkUser, IdentityLinkUserInfo targetLinkUser, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(null))
        {
            if (!await IsLinkedAsync(sourceLinkUser, targetLinkUser, cancellationToken: cancellationToken))
            {
                return;
            }

            var linkedUser = await IdentityLinkUserRepository.FindAsync(sourceLinkUser, targetLinkUser, cancellationToken);
            if (linkedUser != null)
            {
                await IdentityLinkUserRepository.DeleteAsync(linkedUser, cancellationToken: cancellationToken);
            }
        }
    }
    /// <summary>
    /// 生成关联token
    /// </summary>
    /// <param name="targetLinkUser"></param>
    /// <param name="tokenPurpose"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<string> GenerateLinkTokenAsync(IdentityLinkUserInfo targetLinkUser, string tokenPurpose, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(targetLinkUser.TenantId))
        {
            var user = await UserManager.GetByIdAsync(targetLinkUser.UserId);
            return await UserManager.GenerateUserTokenAsync(user, LinkUserTokenProviderConsts.LinkUserTokenProviderName,tokenPurpose);
        }
    }
    /// <summary>
    /// 验证关联token
    /// </summary>
    /// <param name="targetLinkUser"></param>
    /// <param name="token"></param>
    /// <param name="tokenPurpose"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual async Task<bool> VerifyLinkTokenAsync(IdentityLinkUserInfo targetLinkUser, string token, string tokenPurpose, CancellationToken cancellationToken = default)
    {
        using (CurrentTenant.Change(targetLinkUser.TenantId))
        {
            var user = await UserManager.GetByIdAsync(targetLinkUser.UserId);
            return await UserManager.VerifyUserTokenAsync( user,LinkUserTokenProviderConsts.LinkUserTokenProviderName,tokenPurpose,token);
        }
    }
}
