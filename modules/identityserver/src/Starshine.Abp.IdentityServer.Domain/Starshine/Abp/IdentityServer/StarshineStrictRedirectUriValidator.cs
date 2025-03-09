using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Starshine.IdentityServer.Models;
using Starshine.IdentityServer.Validation;
using Volo.Abp.Text.Formatting;

namespace Starshine.Abp.IdentityServer;
/// <summary>
/// 严格重定向 Uri 验证器
/// </summary>
public class StarshineStrictRedirectUriValidator : StrictRedirectUriValidator
{
    /// <summary>
    /// 重定向 Uri 验证器
    /// </summary>
    /// <param name="requestedUri"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    public override async Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
    {
        var isAllowed = await base.IsRedirectUriValidAsync(requestedUri, client);
        return isAllowed || await IsRedirectUriValidWithDomainFormatsAsync(client.RedirectUris, requestedUri);
    }

    /// <summary>
    /// 登出重定向 Uri 验证器
    /// </summary>
    /// <param name="requestedUri"></param>
    /// <param name="client"></param>
    /// <returns></returns>
    public override async Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
    {
        var isAllowed = await base.IsPostLogoutRedirectUriValidAsync(requestedUri, client);
        return isAllowed || await IsRedirectUriValidWithDomainFormatsAsync(client.PostLogoutRedirectUris, requestedUri);
    }

    /// <summary>
    /// 重定向 Uri 验证器
    /// </summary>
    /// <param name="uris"></param>
    /// <param name="requestedUri"></param>
    /// <returns></returns>
    protected virtual Task<bool> IsRedirectUriValidWithDomainFormatsAsync(IEnumerable<string> uris, string requestedUri)
    {
        if (uris == null)
        {
            return Task.FromResult(false);
        }
        foreach (var url in uris)
        {
            var extractResult = FormattedStringValueExtracter.Extract(requestedUri, url, ignoreCase: true);
            if (extractResult.IsMatch)
            {
                return Task.FromResult(extractResult.Matches
                    .Aggregate(url, (current, nameValue) => current.Replace($"{{{nameValue.Name}}}", nameValue.Value))
                    .Contains(requestedUri, StringComparison.OrdinalIgnoreCase));
            }

            if (url.Replace("{0}.", "").Contains(requestedUri, StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}
