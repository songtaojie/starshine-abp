using System.Threading.Tasks;

namespace Starshine.Abp.IdentityServer.Data;
/// <summary>
/// 身份资源数据播种者
/// </summary>
public interface IIdentityResourceDataSeeder
{
    /// <summary>
    /// 创建标准资源
    /// </summary>
    /// <returns></returns>
    Task CreateStandardResourcesAsync();
}
