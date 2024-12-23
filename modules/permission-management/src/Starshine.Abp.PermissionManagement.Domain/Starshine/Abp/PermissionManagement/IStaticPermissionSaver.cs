using System.Threading.Tasks;

namespace Starshine.Abp.PermissionManagement;

public interface IStaticPermissionSaver
{
    Task SaveAsync();
}