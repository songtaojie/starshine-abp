using System.Threading.Tasks;

namespace Starshine.Abp.IdentityServer.IdentityResources;

public interface IIdentityResourceDataSeeder
{
    Task CreateStandardResourcesAsync();
}
