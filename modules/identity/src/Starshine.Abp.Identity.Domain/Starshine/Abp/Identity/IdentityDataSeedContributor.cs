using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份数据种子贡献者
/// </summary>
public class IdentityDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    /// <summary>
    /// 管理员电子邮件属性名称
    /// </summary>
    public const string AdminEmailPropertyName = "AdminEmail";
    /// <summary>
    /// 管理员电子邮件默认值
    /// </summary>
    public const string AdminEmailDefaultValue = "jie797839@gmail.com";
    /// <summary>
    /// 管理员密码属性名称
    /// </summary>
    public const string AdminPasswordPropertyName = "AdminPassword";
    /// <summary>
    /// 管理员密码默认值
    /// </summary>
    public const string AdminPasswordDefaultValue = "1q2w3E*";

    /// <summary>
    /// 身份数据播种者
    /// </summary>
    protected IIdentityDataSeeder IdentityDataSeeder { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityDataSeeder"></param>
    public IdentityDataSeedContributor(IIdentityDataSeeder identityDataSeeder)
    {
        IdentityDataSeeder = identityDataSeeder;
    }

    /// <summary>
    /// 种子数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual Task SeedAsync(DataSeedContext context)
    {
        return IdentityDataSeeder.SeedAsync(
            context?[AdminEmailPropertyName] as string ?? AdminEmailDefaultValue,
            context?[AdminPasswordPropertyName] as string ?? AdminPasswordDefaultValue,
            context?.TenantId
        );
    }
}
