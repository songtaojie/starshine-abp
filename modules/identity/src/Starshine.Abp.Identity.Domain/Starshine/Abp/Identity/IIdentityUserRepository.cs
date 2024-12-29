using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;
/// <summary>
/// 身份用户存储库
/// </summary>
public interface IIdentityUserRepository : IBasicRepository<IdentityUser, Guid>
{
    /// <summary>
    /// 根据名称获取
    /// </summary>
    /// <param name="normalizedUserName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdentityUser?> FindByNormalizedUserNameAsync([NotNull] string normalizedUserName,bool includeDetails = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取用户角色列表
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> GetRoleNamesAsync(Guid id,CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<string>> GetRoleNamesInOrganizationUnitAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检索与指定登录提供程序和登录提供程序密钥关联的用户。
    /// </summary>
    /// <param name="loginProvider">提供 <paramref name="providerKey"/> 的登录提供商。</param>
    /// <param name="providerKey"><paramref name="loginProvider"/> 提供的用于识别用户的密钥。</param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 异步操作的 <see cref="Task"/>，包含与指定登录提供程序和密钥匹配的用户（如果有）。
    /// </returns>
    Task<IdentityUser?> FindByLoginAsync([NotNull] string loginProvider,[NotNull] string providerKey,bool includeDetails = true,CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取与指定的规范化电子邮件地址关联的用户（如果有）。
    /// </summary>
    /// <param name="normalizedEmail">返回用户的规范化电子邮件地址。</param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> 用于传播应取消操作的通知。</param>
    /// <returns>
    /// 包含异步查找操作结果的任务对象，如果有与指定的规范化电子邮件地址相关联的用户。
    /// </returns>
    Task<IdentityUser?> FindByNormalizedEmailAsync([NotNull] string normalizedEmail,bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// 检索具有指定声明的所有用户。
    /// </summary>
    /// <param name="claim">应检索其用户的声明。</param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetListByClaimAsync(Claim claim,bool includeDetails = false,CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="claimType"></param>
    /// <param name="autoSave"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveClaimFromAllUsersAsync(
        string claimType,
        bool autoSave = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="normalizedRoleName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetListByNormalizedRoleNameAsync(
        string normalizedRoleName,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );
    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Guid>> GetUserIdListByRoleIdAsync(
        Guid roleId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="roleId"></param>
    /// <param name="organizationUnitId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="isLockedOut"></param>
    /// <param name="notActive"></param>
    /// <param name="emailConfirmed"></param>
    /// <param name="isExternal"></param>
    /// <param name="maxCreationTime"></param>
    /// <param name="minCreationTime"></param>
    /// <param name="maxModifitionTime"></param>
    /// <param name="minModifitionTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetRolesAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
        Guid id,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetUsersInOrganizationUnitAsync(
        Guid organizationUnitId,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetUsersInOrganizationsListAsync(
        List<Guid> organizationUnitIds,
        CancellationToken cancellationToken = default
        );
    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetUsersInOrganizationUnitWithChildrenAsync(
        string code,
        CancellationToken cancellationToken = default
        );

    /// <summary>
    /// /
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="roleId"></param>
    /// <param name="organizationUnitId"></param>
    /// <param name="userName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="emailAddress"></param>
    /// <param name="name"></param>
    /// <param name="surname"></param>
    /// <param name="isLockedOut"></param>
    /// <param name="notActive"></param>
    /// <param name="emailConfirmed"></param>
    /// <param name="isExternal"></param>
    /// <param name="maxCreationTime"></param>
    /// <param name="minCreationTime"></param>
    /// <param name="maxModifitionTime"></param>
    /// <param name="minModifitionTime"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetCountAsync(
        string? filter = null,
        Guid? roleId = null,
        Guid? organizationUnitId = null,
        string? userName = null,
        string? phoneNumber = null,
        string? emailAddress = null,
        string? name = null,
        string? surname = null,
        bool? isLockedOut = null,
        bool? notActive = null,
        bool? emailConfirmed = null,
        bool? isExternal = null,
        DateTime? maxCreationTime = null,
        DateTime? minCreationTime = null,
        DateTime? maxModifitionTime = null,
        DateTime? minModifitionTime = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="tenantId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IdentityUser> FindByTenantIdAndUserNameAsync(
        [NotNull] string userName,
        Guid? tenantId,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetListByIdsAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceRoleId"></param>
    /// <param name="targetRoleId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateRoleAsync(
        Guid sourceRoleId,
        Guid? targetRoleId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceOrganizationId"></param>
    /// <param name="targetOrganizationId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateOrganizationAsync(
        Guid sourceOrganizationId,
        Guid? targetOrganizationId,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userIds"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUserIdWithRoleNames>> GetRoleNamesAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken = default);
}
