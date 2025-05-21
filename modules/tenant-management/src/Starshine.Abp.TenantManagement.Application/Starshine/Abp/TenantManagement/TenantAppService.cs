using Microsoft.AspNetCore.Authorization;
using Starshine.Abp.Application.Dtos;
using Starshine.Abp.TenantManagement.Dtos;
using Starshine.Abp.TenantManagement.Entities;
using Starshine.Abp.TenantManagement.Managers;
using Starshine.Abp.TenantManagement.Repositories;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.EventBus.Local;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 租户应用服务
/// </summary>
/// <param name="tenantRepository"></param>
/// <param name="tenantManager"></param>
/// <param name="dataSeeder"></param>
/// <param name="distributedEventBus"></param>
/// <param name="localEventBus"></param>
[Authorize(TenantManagementPermissions.Tenants.Default)]
public class TenantAppService(
    ITenantRepository tenantRepository,
    ITenantManager tenantManager,
    IDataSeeder dataSeeder,
    IDistributedEventBus distributedEventBus,
    ILocalEventBus localEventBus,
    IAbpLazyServiceProvider abpLazyServiceProvider) : TenantManagementAppServiceBase(abpLazyServiceProvider), ITenantAppService
{
    /// <summary>
    /// 数据种子
    /// </summary>
    protected IDataSeeder DataSeeder { get; } = dataSeeder;
    /// <summary>
    /// 租户仓库
    /// </summary>
    protected ITenantRepository TenantRepository { get; } = tenantRepository;
    /// <summary>
    /// 租户管理
    /// </summary>
    protected ITenantManager TenantManager { get; } = tenantManager;
    /// <summary>
    /// 分布式事件总线
    /// </summary>
    protected IDistributedEventBus DistributedEventBus { get; } = distributedEventBus;
    /// <summary>
    /// 本地事件总线
    /// </summary>
    protected ILocalEventBus LocalEventBus { get; } = localEventBus;

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<TenantDto> GetAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        return new TenantDto
        { 
            ConcurrencyStamp = tenant.ConcurrencyStamp,
            Name = tenant.Name,
            Id = tenant.Id    
        };
    }

    /// <summary>
    /// 获取租户列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual async Task<PagedResultDto<TenantDto>> GetListAsync(TenantsRequestDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Tenant.Name);
        }

        var count = await TenantRepository.GetCountAsync(input.Filter);
        var list = await TenantRepository.GetListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.Filter
        );

        var result = list.ConvertAll(t => new TenantDto
        {
            ConcurrencyStamp = t.ConcurrencyStamp,
            Name = t.Name,
            Id = t.Id
        });
        return new PagedResultDto<TenantDto>(count,result);
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Create)]
    public virtual async Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        var tenant = await TenantManager.CreateAsync(input.Name);
        input.MapExtraPropertiesTo(tenant);

        await TenantRepository.InsertAsync(tenant);

        await CurrentUnitOfWork!.SaveChangesAsync();

        await DistributedEventBus.PublishAsync(
            new TenantCreatedEto
            {
                Id = tenant.Id,
                Name = tenant.Name,
                Properties =
                {
                    { "AdminEmail", input.AdminEmailAddress },
                    { "AdminPassword", input.AdminPassword }
                }
            });

        using (CurrentTenant.Change(tenant.Id, tenant.Name))
        {
            //TODO: Handle database creation?
            // TODO: Seeder might be triggered via event handler.
            await DataSeeder.SeedAsync(
                            new DataSeedContext(tenant.Id)
                                .WithProperty("AdminEmail", input.AdminEmailAddress)
                                .WithProperty("AdminPassword", input.AdminPassword)
                            );
        }

        return new TenantDto
        { 
            ConcurrencyStamp = tenant.ConcurrencyStamp,
            Name = tenant.Name,
            Id = tenant.Id
        };
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Update)]
    public virtual async Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        var tenant = await TenantRepository.GetAsync(id);

        await TenantManager.ChangeNameAsync(tenant, input.Name);

        tenant.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);
        input.MapExtraPropertiesTo(tenant);

        await TenantRepository.UpdateAsync(tenant);

        return new TenantDto
        {
            ConcurrencyStamp = tenant.ConcurrencyStamp,
            Name = tenant.Name,
            Id = tenant.Id
        };
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        var tenant = await TenantRepository.FindAsync(id);
        if (tenant == null)
        {
            return;
        }

        await TenantRepository.DeleteAsync(tenant);
    }

    /// <summary>
    /// 获取默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task<string?> GetDefaultConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        return tenant?.FindDefaultConnectionString();
    }

    /// <summary>
    /// 更新默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultConnectionString"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
    {
        var tenant = await TenantRepository.GetAsync(id);
        if (tenant.FindDefaultConnectionString() != defaultConnectionString)
        {
            await LocalEventBus.PublishAsync(new TenantChangedEvent(tenant.Id, tenant.NormalizedName));
        }
        tenant.SetDefaultConnectionString(defaultConnectionString);
        await TenantRepository.UpdateAsync(tenant);
    }

    /// <summary>
    /// 删除默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(TenantManagementPermissions.Tenants.ManageConnectionStrings)]
    public virtual async Task DeleteDefaultConnectionStringAsync(Guid id)
    {
        var tenant = await TenantRepository.GetAsync(id);
        tenant.RemoveDefaultConnectionString();
        await LocalEventBus.PublishAsync(new TenantChangedEvent(tenant.Id, tenant.NormalizedName));
        await TenantRepository.UpdateAsync(tenant);
    }
}
