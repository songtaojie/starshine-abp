using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Starshine.Abp.Identity;

/// <summary>
/// 组织单位存储库
/// </summary>
public interface IOrganizationUnitRepository : IBasicRepository<OrganizationUnit, Guid>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetChildrenAsync(
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="code"></param>
    /// <param name="parentId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetAllChildrenWithParentCodeAsync(
        string? code,
        Guid? parentId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OrganizationUnit> GetAsync(
        string displayName,
        bool includeDetails = true,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetListAsync(
        IEnumerable<Guid> ids,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<OrganizationUnit>> GetListByRoleIdAsync(
        Guid roleId,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetRolesAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnitIds"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetRolesAsync(
        Guid[] organizationUnitIds,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetRolesCountAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityRole>> GetUnaddedRolesAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetUnaddedRolesCountAsync(
        OrganizationUnit organizationUnit,
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetMembersAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<Guid>> GetMemberIdsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetMembersCountAsync(
        OrganizationUnit organizationUnit,
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="sorting"></param>
    /// <param name="maxResultCount"></param>
    /// <param name="skipCount"></param>
    /// <param name="filter"></param>
    /// <param name="includeDetails"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<IdentityUser>> GetUnaddedUsersAsync(
        OrganizationUnit organizationUnit,
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        string? filter = null,
        bool includeDetails = false,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> GetUnaddedUsersCountAsync(
        OrganizationUnit organizationUnit,
        string? filter = null,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAllRolesAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default
    );

    /// <summary>
    /// /
    /// </summary>
    /// <param name="organizationUnit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAllMembersAsync(
        OrganizationUnit organizationUnit,
        CancellationToken cancellationToken = default
    );
}
