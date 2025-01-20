using System.Threading.Tasks;

namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 静态权限保存
/// </summary>
public interface IStaticPermissionSaver
{
    /// <summary>
    /// 保存
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}