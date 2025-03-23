using System.Threading.Tasks;

namespace Starshine.Abp.OpenIddict;

public interface IAbpOpenIddictClaimsPrincipalHandler
{
    Task HandleAsync(AbpOpenIddictClaimsPrincipalHandlerContext context);
}
