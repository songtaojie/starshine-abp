using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份声明类型管理器
/// </summary>
public class IdentityClaimTypeManager : DomainService
{
    /// <summary>
    /// 身份声明类型存储库
    /// </summary>
    protected IIdentityClaimTypeRepository IdentityClaimTypeRepository { get; }
    /// <summary>
    /// 身份用户存储库
    /// </summary>
    protected IIdentityUserRepository IdentityUserRepository { get; }
    /// <summary>
    /// IdentityRole 存储库
    /// </summary>
    protected IIdentityRoleRepository IdentityRoleRepository { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lazyServiceProvider"></param>
    /// <param name="identityClaimTypeRepository"></param>
    /// <param name="identityUserRepository"></param>
    /// <param name="identityRoleRepository"></param>
    public IdentityClaimTypeManager(
        IAbpLazyServiceProvider lazyServiceProvider,
        IIdentityClaimTypeRepository identityClaimTypeRepository,
        IIdentityUserRepository identityUserRepository,
        IIdentityRoleRepository identityRoleRepository)
    {
        IdentityClaimTypeRepository = identityClaimTypeRepository;
        IdentityUserRepository = identityUserRepository;
        IdentityRoleRepository = identityRoleRepository;
        base.LazyServiceProvider = lazyServiceProvider;
    }

    /// <summary>
    /// 创建声明类型
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    public virtual async Task<IdentityClaimType> CreateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name))
        {
            throw new BusinessException(IdentityErrorCodes.ClaimNameExist).WithData("0", claimType.Name);
        }

        return await IdentityClaimTypeRepository.InsertAsync(claimType);
    }

    /// <summary>
    /// 更新声明类型
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    public virtual async Task<IdentityClaimType> UpdateAsync(IdentityClaimType claimType)
    {
        if (await IdentityClaimTypeRepository.AnyAsync(claimType.Name, claimType.Id))
        {
            throw new BusinessException($"名称({claimType.Name})已存在");
        }

        if (claimType.IsStatic)
        {
            throw new BusinessException($"无法更新静态 ClaimType.");
        }

        return await IdentityClaimTypeRepository.UpdateAsync(claimType);
    }

    /// <summary>
    /// 删除声明类型
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    public virtual async Task DeleteAsync(Guid id)
    {
        var claimType = await IdentityClaimTypeRepository.GetAsync(id);
        if (claimType.IsStatic)
        {
            throw new AbpException($"无法删除静态 ClaimType。");
        }

        //从所有用户和角色中删除此类型的声明
        await IdentityUserRepository.RemoveClaimFromAllUsersAsync(claimType.Name);
        await IdentityRoleRepository.RemoveClaimFromAllRolesAsync(claimType.Name);

        await IdentityClaimTypeRepository.DeleteAsync(id);
    }
}
