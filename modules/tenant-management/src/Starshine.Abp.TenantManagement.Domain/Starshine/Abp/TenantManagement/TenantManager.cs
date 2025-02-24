using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户管理
/// </summary>
public class TenantManager : DomainService, ITenantManager
{
    /// <summary>
    /// 租户存储
    /// </summary>
    protected ITenantRepository TenantRepository { get; }
    /// <summary>
    /// 租户标准化
    /// </summary>
    protected ITenantNormalizer TenantNormalizer { get; }
    /// <summary>
    /// 本地事件总线
    /// </summary>
    protected ILocalEventBus LocalEventBus { get; }

    /// <summary>
    /// 租户管理
    /// </summary>
    /// <param name="tenantRepository"></param>
    /// <param name="tenantNormalizer"></param>
    /// <param name="localEventBus"></param>
    public TenantManager(
        ITenantRepository tenantRepository,
        ITenantNormalizer tenantNormalizer,
        ILocalEventBus localEventBus)
    {
        TenantRepository = tenantRepository;
        TenantNormalizer = tenantNormalizer;
        LocalEventBus = localEventBus;
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual async Task<Tenant> CreateAsync(string name)
    {
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name)!;
        await ValidateNameAsync(normalizedName);
        return new Tenant(GuidGenerator.Create(), name, normalizedName);
    }

    /// <summary>
    /// 更改名称
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public virtual async Task ChangeNameAsync(Tenant tenant, string name)
    {
        Check.NotNull(tenant, nameof(tenant));
        Check.NotNull(name, nameof(name));

        var normalizedName = TenantNormalizer.NormalizeName(name)!;

        await ValidateNameAsync(normalizedName, tenant.Id);
        await LocalEventBus.PublishAsync(new TenantChangedEvent(tenant.Id, tenant.NormalizedName));
        tenant.SetName(name);
        tenant.SetNormalizedName(normalizedName);
    }

    /// <summary>
    /// 验证名称
    /// </summary>
    /// <param name="normalizeName"></param>
    /// <param name="expectedId"></param>
    /// <returns></returns>
    protected virtual async Task ValidateNameAsync(string normalizeName, Guid? expectedId = null)
    {
        var tenant = await TenantRepository.FindByNameAsync(normalizeName);
        if (tenant != null && tenant.Id != expectedId)
        {
            throw new BusinessException("Starshine.Abp.TenantManagement:DuplicateTenantName").WithData("Name", normalizeName);
        }
    }
}
