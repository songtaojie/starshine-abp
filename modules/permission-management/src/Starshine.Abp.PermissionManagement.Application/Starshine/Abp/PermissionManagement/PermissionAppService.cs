using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SimpleStateChecking;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 权限应用服务
/// </summary>
[Authorize]
public class PermissionAppService : ApplicationService, IPermissionAppService
{
    /// <summary>
    /// 权限管理配置
    /// </summary>
    protected PermissionManagementOptions Options { get; }
    /// <summary>
    /// 权限管理
    /// </summary>
    protected IPermissionManager PermissionManager { get; }
    /// <summary>
    /// 权限定义管理
    /// </summary>
    protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
    /// <summary>
    /// 状态检查管理
    /// </summary>
    protected ISimpleStateCheckerManager<PermissionDefinition> SimpleStateCheckerManager { get; }

    /// <summary>
    /// 权限应用服务
    /// </summary>
    /// <param name="permissionManager">权限管理</param>
    /// <param name="permissionDefinitionManager">权限定义管理</param>
    /// <param name="options">权限配置</param>
    /// <param name="simpleStateCheckerManager">状态检查管理</param>
    /// <param name="abpLazyServiceProvider">服务提供商</param>
    public PermissionAppService(
        IPermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
        IAbpLazyServiceProvider abpLazyServiceProvider)
    {
        Options = options.Value;
        PermissionManager = permissionManager;
        PermissionDefinitionManager = permissionDefinitionManager;
        SimpleStateCheckerManager = simpleStateCheckerManager;
        LazyServiceProvider = abpLazyServiceProvider;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <returns></returns>
    public virtual async Task<GetPermissionListResultDto> GetAsync(string providerName, string providerKey)
    {
        await CheckProviderPolicy(providerName);

        var result = new GetPermissionListResultDto
        {
            EntityDisplayName = providerKey,
            Groups = []
        };

        var multiTenancySide = CurrentTenant.GetMultiTenancySide();

        foreach (var group in await PermissionDefinitionManager.GetGroupsAsync())
        {
            var groupDto = CreatePermissionGroupDto(group);

            var neededCheckPermissions = new List<PermissionDefinition>();

            var permissions = group.GetPermissionsWithChildren()
                .Where(x => x.IsEnabled)
                .Where(x => x.Providers.Count == 0 || x.Providers.Contains(providerName))
                .Where(x => x.MultiTenancySide.HasFlag(multiTenancySide));

            foreach (var permission in permissions)
            {
                if (permission.Parent != null && !neededCheckPermissions.Contains(permission.Parent))
                {
                    continue;
                }

                if (await SimpleStateCheckerManager.IsEnabledAsync(permission))
                {
                    neededCheckPermissions.Add(permission);
                }
            }

            if (neededCheckPermissions.Count == 0)
            {
                continue;
            }

            var grantInfoDtos = neededCheckPermissions
                .Select(CreatePermissionGrantInfoDto)
                .ToList();

            var multipleGrantInfo = await PermissionManager.GetAsync(neededCheckPermissions.Select(x => x.Name).ToArray(), providerName, providerKey);

            foreach (var grantInfo in multipleGrantInfo.Result)
            {
                var grantInfoDto = grantInfoDtos.First(x => x.Name == grantInfo.Name);

                grantInfoDto.IsGranted = grantInfo.IsGranted;

                foreach (var provider in grantInfo.Providers)
                {
                    grantInfoDto.GrantedProviders!.Add(new ProviderInfoDto
                    {
                        ProviderName = provider.Name,
                        ProviderKey = provider.Key,
                    });
                }

                groupDto.Permissions!.Add(grantInfoDto);
            }

            if (groupDto.Permissions!.Any())
            {
                result.Groups.Add(groupDto);
            }
        }

        return result;
    }

    /// <summary>
    /// 创建权限授予信息
    /// </summary>
    /// <param name="permission">权限定义</param>
    /// <returns></returns>
    private PermissionGrantInfoDto CreatePermissionGrantInfoDto(PermissionDefinition permission)
    {
        return new PermissionGrantInfoDto
        {
            Name = permission.Name,
            DisplayName = permission.DisplayName == null? string.Empty: permission.DisplayName.Localize(StringLocalizerFactory),
            ParentName = permission.Parent?.Name,
            AllowedProviders = permission.Providers,
            GrantedProviders = []
        };
    }

    /// <summary>
    /// 创建权限组
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    private PermissionGroupDto CreatePermissionGroupDto(PermissionGroupDefinition group)
    {
        var localizableDisplayName = group.DisplayName as LocalizableString;

        return new PermissionGroupDto
        {
            Name = group.Name,
            DisplayName = group.DisplayName.Localize(StringLocalizerFactory),
            DisplayNameKey = localizableDisplayName?.Name,
            DisplayNameResource = localizableDisplayName?.ResourceType != null
                ? LocalizationResourceNameAttribute.GetName(localizableDisplayName.ResourceType)
                : null,
            Permissions = []
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="providerName"></param>
    /// <param name="providerKey"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
    {
        await CheckProviderPolicy(providerName);

        foreach (var permissionDto in input.Permissions)
        {
            await PermissionManager.SetAsync(permissionDto.Name, providerName, providerKey, permissionDto.IsGranted);
        }
    }

    /// <summary>
    /// 检查提供者策略
    /// </summary>
    /// <param name="providerName">提供者名称</param>
    /// <returns></returns>
    /// <exception cref="AbpException"></exception>
    protected virtual async Task CheckProviderPolicy(string providerName)
    {
        var policyName = Options.ProviderPolicies.GetOrDefault(providerName);
        if (policyName.IsNullOrEmpty())
        {
            throw new BusinessException($"没有为提供程序'{providerName}'定义获取/设置权限的策略。 使用{nameof(PermissionManagementOptions)}来配置策略。");
        }

        await AuthorizationService.CheckAsync(policyName);
    }
}
