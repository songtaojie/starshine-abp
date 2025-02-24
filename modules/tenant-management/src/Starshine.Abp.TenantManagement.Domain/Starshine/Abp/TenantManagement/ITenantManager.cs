using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Services;

namespace Starshine.Abp.TenantManagement;
/// <summary>
/// 租户管理器
/// </summary>
public interface ITenantManager : IDomainService
{
    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [NotNull]
    Task<Tenant> CreateAsync([NotNull] string name);

    /// <summary>
    /// 更新租户名称
    /// </summary>
    /// <param name="tenant"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Task ChangeNameAsync([NotNull] Tenant tenant, [NotNull] string name);
}
