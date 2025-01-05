using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ObjectExtending;

namespace Starshine.Abp.Identity;
/// <summary>
/// 
/// </summary>
[Authorize(IdentityPermissions.Users.Default)]
public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
{
    /// <summary>
    /// 
    /// </summary>
    protected IdentityUserManager UserManager { get; }
    /// <summary>
    /// 
    /// </summary>
    protected IIdentityUserRepository UserRepository { get; }
    /// <summary>
    /// /
    /// </summary>
    protected IIdentityRoleRepository RoleRepository { get; }
    /// <summary>
    /// 
    /// </summary>
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    /// <summary>
    /// 
    /// </summary>
    protected IPermissionChecker PermissionChecker { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="userRepository"></param>
    /// <param name="roleRepository"></param>
    /// <param name="identityOptions"></param>
    /// <param name="permissionChecker"></param>
    /// <param name="abpLazyServiceProvider"></param>
    public IdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions,
        IPermissionChecker permissionChecker,
        IAbpLazyServiceProvider abpLazyServiceProvider):base(abpLazyServiceProvider)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        IdentityOptions = identityOptions;
        PermissionChecker = permissionChecker;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUserDto> GetAsync(Guid id)
    {
        return (await UserManager.GetByIdAsync(id)).ToIdentityUserDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        var count = await UserRepository.GetCountAsync(input.Filter);
        var list = await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);
        return new PagedResultDto<IdentityUserDto>(count,list.ConvertAll(DtoExtensions.ToIdentityUserDto));
    }
   
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        //TODO: 还应包括相关 OU 的角色。
        var roles = await UserRepository.GetRolesAsync(id);
        var identityRoleDtos = roles.ConvertAll(role => new IdentityRoleDto
        {
            ConcurrencyStamp = role.ConcurrencyStamp,
            Id = role.Id,
            Name = role.Name,
            IsDefault = role.IsDefault,
            IsPublic = role.IsPublic,
            IsStatic = role.IsStatic
        });
        return new ListResultDto<IdentityRoleDto>(identityRoleDtos);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        var list = await RoleRepository.GetListAsync();
        return new ListResultDto<IdentityRoleDto>(list.ConvertAll(DtoExtensions.ToIdentityRoleDto));
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(IdentityPermissions.Users.Create)]
    public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        );

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UpdateUserByInput(user, input);
        (await UserManager.UpdateAsync(user)).CheckErrors();
        if(CurrentUnitOfWork != null) await CurrentUnitOfWork.SaveChangesAsync();

        return user.ToIdentityUserDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

        await UpdateUserByInput(user, input);
        input.MapExtraPropertiesTo(user);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        if (!input.Password.IsNullOrEmpty())
        {
            (await UserManager.RemovePasswordAsync(user)).CheckErrors();
            (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
        }
        if(CurrentUnitOfWork != null) await CurrentUnitOfWork.SaveChangesAsync();

        return user.ToIdentityUserDto();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BusinessException"></exception>
    [Authorize(IdentityPermissions.Users.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        if (CurrentUser.Id == id)
        {
            throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
        }

        var user = await UserManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return;
        }

        (await UserManager.DeleteAsync(user)).CheckErrors();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        await IdentityOptions.SetAsync();
        var user = await UserManager.GetByIdAsync(id);
        (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        await UserRepository.UpdateAsync(user);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUserDto?> FindByUsernameAsync(string userName)
    {
        var identityUser = await UserManager.FindByNameAsync(userName);
        if (identityUser == null) return null;
        return identityUser.ToIdentityUserDto();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public virtual async Task<IdentityUserDto?> FindByEmailAsync(string email)
    {
        var identityUser = await UserManager.FindByEmailAsync(email);
        if (identityUser == null) return null;
        return identityUser.ToIdentityUserDto();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }


        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        if (user.Id != CurrentUser.Id)
        {
            (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();
        }

        user.Name = input.Name;
        user.Surname = input.Surname;
        (await UserManager.UpdateAsync(user)).CheckErrors();
        user.SetIsActive(input.IsActive);
        if (input.RoleNames != null && await PermissionChecker.IsGrantedAsync(IdentityPermissions.Users.ManageRoles))
        {
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }
    }
}
