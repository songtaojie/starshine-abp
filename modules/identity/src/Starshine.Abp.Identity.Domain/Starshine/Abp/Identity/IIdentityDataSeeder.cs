using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份数据播种者
/// </summary>
public interface IIdentityDataSeeder
{
    /// <summary>
    /// 设置种子数据
    /// </summary>
    /// <param name="adminEmail"></param>
    /// <param name="adminPassword"></param>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    Task<IdentityDataSeedResult> SeedAsync([NotNull] string adminEmail,[NotNull] string adminPassword,Guid? tenantId = null);
}
