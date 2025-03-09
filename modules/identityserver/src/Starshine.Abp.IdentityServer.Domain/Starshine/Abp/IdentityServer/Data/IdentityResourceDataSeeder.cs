using Starshine.Abp.Identity;
using Starshine.Abp.IdentityServer.Entities;
using Starshine.Abp.IdentityServer.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Starshine.Abp.IdentityServer.Data;
/// <summary>
/// 身份资源数据播种者
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="identityResourceRepository"></param>
/// <param name="guidGenerator"></param>
/// <param name="claimTypeRepository"></param>
public class IdentityResourceDataSeeder(
    IIdentityResourceRepository identityResourceRepository,
    IGuidGenerator guidGenerator,
    IIdentityClaimTypeRepository claimTypeRepository) : IIdentityResourceDataSeeder, ITransientDependency
{
    /// <summary>
    /// 身份声明类型存储库
    /// </summary>
    protected IIdentityClaimTypeRepository ClaimTypeRepository { get; } = claimTypeRepository;

    /// <summary>
    /// 身份资源存储库
    /// </summary>
    protected IIdentityResourceRepository IdentityResourceRepository { get; } = identityResourceRepository;

    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; } = guidGenerator;

    /// <summary>
    /// 创建标准资源
    /// </summary>
    /// <returns></returns>
    public virtual async Task CreateStandardResourcesAsync()
    {
        var resources = new[]
        {
            new Starshine.IdentityServer.Models.IdentityResources.OpenId(),
            new Starshine.IdentityServer.Models.IdentityResources.Profile(),
            new Starshine.IdentityServer.Models.IdentityResources.Email(),
            new Starshine.IdentityServer.Models.IdentityResources.Address(),
            new Starshine.IdentityServer.Models.IdentityResources.Phone(),
            new Starshine.IdentityServer.Models.IdentityResource("role", "Roles of the user", new[] {"role"})
        };

        foreach (var resource in resources)
        {
            foreach (var claimType in resource.UserClaims)
            {
                await AddClaimTypeIfNotExistsAsync(claimType);
            }

            await AddIdentityResourceIfNotExistsAsync(resource);
        }
    }

    /// <summary>
    /// 如果不存在则添加身份资源
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    protected virtual async Task AddIdentityResourceIfNotExistsAsync(Starshine.IdentityServer.Models.IdentityResource resource)
    {
        if (await IdentityResourceRepository.CheckNameExistAsync(resource.Name))
        {
            return;
        }
        var identityResourceId = GuidGenerator.Create();
        await IdentityResourceRepository.InsertAsync(
            new IdentityResource(identityResourceId)
            {
                Name = resource.Name,
                DisplayName = resource.DisplayName,
                Description = resource.Description,
                Enabled = resource.Enabled,
                Required = resource.Required,
                Emphasize = resource.Emphasize,
                ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument,
                UserClaims = resource.UserClaims.Select(claimType => new IdentityResourceClaim
                {
                    IdentityResourceId = identityResourceId,
                    Type = claimType
                }).ToList(),
                Properties = resource.Properties.Select(x => new IdentityResourceProperty
                {
                    IdentityResourceId = identityResourceId,
                    Key = x.Key,
                    Value = x.Value
                }).ToList(),
            }
        );
    }

    /// <summary>
    /// 如果不存在则添加身份声明类型
    /// </summary>
    /// <param name="claimType"></param>
    /// <returns></returns>
    protected virtual async Task AddClaimTypeIfNotExistsAsync(string claimType)
    {
        if (await ClaimTypeRepository.AnyAsync(claimType))
        {
            return;
        }

        await ClaimTypeRepository.InsertAsync(
            new IdentityClaimType(
                GuidGenerator.Create(),
                claimType,
                isStatic: true
            )
        );
    }
}
