using System;
using System.Linq;
using System.Threading.Tasks;
using Starshine.IdentityServer.Configuration;
using Starshine.IdentityServer.Validation;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 客户端配置验证器
/// </summary>
public class StarshineClientConfigurationValidator : DefaultClientConfigurationValidator
{
    /// <summary>
    /// 客户端配置验证器
    /// </summary>
    /// <param name="options"></param>
    public StarshineClientConfigurationValidator(IdentityServerOptions options)
        : base(options)
    {
    }
    /// <summary>
    /// 验证跨域
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    protected override Task ValidateAllowedCorsOriginsAsync(ClientConfigurationValidationContext context)
    {
        context.Client.AllowedCorsOrigins = context.Client
            .AllowedCorsOrigins.Select(x => x.Replace("{0}.", string.Empty, StringComparison.OrdinalIgnoreCase))
            .ToHashSet();

        return base.ValidateAllowedCorsOriginsAsync(context);
    }
}
