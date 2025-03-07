using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Starshine.Abp.TenantManagement;

/// <summary>
/// 租户应用服务
/// </summary>
public interface ITenantAppService : ICrudAppService<TenantDto, Guid, TenantsRequestDto, TenantCreateDto, TenantUpdateDto>
{
    /// <summary>
    /// 获取租户默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<string?> GetDefaultConnectionStringAsync(Guid id);

    /// <summary>
    /// 更新租户默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultConnectionString"></param>
    /// <returns></returns>
    Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString);

    /// <summary>
    /// 删除租户默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteDefaultConnectionStringAsync(Guid id);
}
