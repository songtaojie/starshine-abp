using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Starshine.Abp.Identity;

/// <summary>
/// 为角色创建持久性存储的新实例。
/// </summary>
public class IdentityRoleStore : IRoleStore<IdentityRole>, IRoleClaimStore<IdentityRole>, ITransientDependency
{
    /// <summary>
    /// 角色存储库
    /// </summary>
    protected IIdentityRoleRepository RoleRepository { get; }
    /// <summary>
    /// 日志记录
    /// </summary>
    protected ILogger<IdentityRoleStore> Logger { get; }
    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }

    /// <summary>
    ///构造 <see cref="IdentityRoleStore"/> 的新实例。
    /// </summary>
    public IdentityRoleStore(IIdentityRoleRepository roleRepository,ILogger<IdentityRoleStore> logger,
        IGuidGenerator guidGenerator,IdentityErrorDescriber? describer = null)
    {
        RoleRepository = roleRepository;
        Logger = logger;
        GuidGenerator = guidGenerator;
        ErrorDescriber = describer ?? new IdentityErrorDescriber();
    }

    /// <summary>
    /// 获取或设置当前操作发生的任何错误的 <see cref="IdentityErrorDescriber"/>。
    /// </summary>
    public IdentityErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    /// 获取或设置一个标志，指示在调用 CreateAsync、UpdateAsync 和 DeleteAsync 后是否应保留更改。
    /// </summary>
    /// <value>
    /// 如果应自动保留更改，则为 True，否则为 false。
    /// </value>
    public bool AutoSaveChanges { get; set; } = true;

    /// <summary>
    /// 作为异步操作在商店中创建新角色。
    /// </summary>
    /// <param name="role">在存储中创建的角色.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 表示异步查询的 <see cref="IdentityResult"/>.</returns>
    public virtual async Task<IdentityResult> CreateAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        await RoleRepository.InsertAsync(role, AutoSaveChanges, cancellationToken);
        return IdentityResult.Success;
    }

    /// <summary>
    /// 作为异步操作更新store中的角色。
    /// </summary>
    /// <param name="role">store中要更新的角色。</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> 用于传播应取消操作的通知.</param>
    /// <returns>A <see cref="Task{TResult}"/> 表示异步查询的 <see cref="IdentityResult"/>.</returns>
    public virtual async Task<IdentityResult> UpdateAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        try
        {
            await RoleRepository.UpdateAsync(role, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            Logger.LogError(ex,"更新角色失败"); //触发一些 AbpHandledException 事件
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// 作为异步操作从存储中删除角色
    /// </summary>
    /// <param name="role">store中要删除的角色。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 代表异步查询的 <see cref="IdentityResult"/>。</returns>
    public virtual async Task<IdentityResult> DeleteAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        try
        {
            await RoleRepository.DeleteAsync(role, AutoSaveChanges, cancellationToken);
        }
        catch (AbpDbConcurrencyException ex)
        {
            Logger.LogError(ex,"删除角色失败");  //触发一些 AbpHandledException 事件
            return IdentityResult.Failed(ErrorDescriber.ConcurrencyFailure());
        }

        return IdentityResult.Success;
    }

    /// <summary>
    /// 作为异步操作从存储中获取角色的 ID。
    /// </summary>
    /// <param name="role">应返回其 ID 的角色.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 包含角色 ID.</returns>
    public virtual Task<string> GetRoleIdAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        return Task.FromResult(role.Id.ToString());
    }

    /// <summary>
    /// 作为异步操作从存储中获取角色的名称。
    /// </summary>
    /// <param name="role">应返回其名称的角色.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 包含角色的名称。</returns>
    public virtual Task<string?> GetRoleNameAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        return Task.FromResult<string?>(role.Name);
    }

    /// <summary>
    /// 将store中角色的名称设置为异步操作。
    /// </summary>
    /// <param name="role">应设置角色名称.</param>
    /// <param name="roleName">角色名称.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>The <see cref="Task"/> 代表异步操作。</returns>
    public virtual Task SetRoleNameAsync([NotNull] IdentityRole role, string? roleName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        Check.NotNull(roleName, nameof(roleName));
        role.ChangeName(roleName);
        return Task.CompletedTask;
    }

    /// <summary>
    /// 通过异步操作查找具有指定ID的角色。
    /// </summary>
    /// <param name="id">要查找的角色 ID。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 查找的结果.</returns>
    public virtual Task<IdentityRole?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return RoleRepository.FindAsync(Guid.Parse(id), cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 通过异步操作查找具有指定规范化名称的角色。
    /// </summary>
    /// <param name="normalizedName">要查找的规范化角色名称。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>A <see cref="Task{TResult}"/> 查找的结果。</returns>
    public virtual Task<IdentityRole?> FindByNameAsync([NotNull] string normalizedName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(normalizedName, nameof(normalizedName));
        return RoleRepository.FindByNormalizedNameAsync(normalizedName, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// 以异步操作的方式获取角色的规范化名称。
    /// </summary>
    /// <param name="role">应检索其规范化名称的角色。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>包含角色名称的 <see cref="Task{TResult}"/>。</returns>
    public virtual Task<string?> GetNormalizedRoleNameAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        return Task.FromResult(role.NormalizedName);
    }

    /// <summary>
    /// 将角色的规范化名称设置为异步操作。
    /// </summary>
    /// <param name="role">应设置规范化名称的角色。</param>
    /// <param name="normalizedName">要设置的规范化名称</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>The <see cref="Task"/> 代表异步操作。</returns>
    public virtual Task SetNormalizedRoleNameAsync([NotNull] IdentityRole role, string? normalizedName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        role.NormalizedName = normalizedName;
        return Task.CompletedTask;
    }

    /// <summary>
    /// 释放stores
    /// </summary>
    public virtual void Dispose()
    {
    }

    /// <summary>
    /// 通过异步操作获取与指定 <paramref name="role"/> 关联的声明。
    /// </summary>
    /// <param name="role">应检索其声明的角色。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>包含授予角色的声明的 <see cref="Task{TResult}"/>。</returns>
    public virtual async Task<IList<Claim>> GetClaimsAsync([NotNull] IdentityRole role, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        await RoleRepository.EnsureCollectionLoadedAsync(role, r => r.Claims, cancellationToken);
        return role.Claims.Select(c => c.ToClaim()).ToList();
    }

    /// <summary>
    /// 添加赋予指定 <paramref name="claim"/> 的 <paramref name="role"/>。
    /// </summary>
    /// <param name="role">要添加声明的角色。</param>
    /// <param name="claim">添加到角色的声明。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task AddClaimAsync([NotNull] IdentityRole role, [NotNull] Claim claim, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Check.NotNull(role, nameof(role));
        Check.NotNull(claim, nameof(claim));
        await RoleRepository.EnsureCollectionLoadedAsync(role, r => r.Claims, cancellationToken);
        role.AddClaim(GuidGenerator, claim);
    }

    /// <summary>
    /// 从指定的 <paramref name="role"/> 中删除给出的 <paramref name="claim"/>。
    /// </summary>
    /// <param name="role">要删除声明的角色。</param>
    /// <param name="claim">要求从角色中移除。</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>代表异步操作的 <see cref="Task"/>。</returns>
    public virtual async Task RemoveClaimAsync([NotNull] IdentityRole role, [NotNull] Claim claim, CancellationToken cancellationToken = default)
    {
        Check.NotNull(role, nameof(role));
        Check.NotNull(claim, nameof(claim));
        await RoleRepository.EnsureCollectionLoadedAsync(role, r => r.Claims, cancellationToken);
        role.RemoveClaim(claim);
    }
}
