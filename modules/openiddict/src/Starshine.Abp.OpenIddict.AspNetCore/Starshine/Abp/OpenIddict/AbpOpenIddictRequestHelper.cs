﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OpenIddict.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.OpenIddict;

public class AbpOpenIddictRequestHelper : ITransientDependency
{
    public virtual Task<OpenIddictRequest?> GetFromReturnUrlAsync(string returnUrl)
    {
        if (!returnUrl.IsNullOrWhiteSpace())
        {
            var qm = returnUrl.IndexOf('?');
            if (qm > 0)
            {
                return Task.FromResult<OpenIddictRequest?>(new OpenIddictRequest(returnUrl.Substring(qm + 1)
                    .Split("&")
                    .Select(x =>
                        x.Split("=").Length == 2
                            ? new KeyValuePair<string, string?>(x.Split("=")[0], WebUtility.UrlDecode(x.Split("=")[1]))
                            : new KeyValuePair<string, string?>(string.Empty, null))
                    .Where(x => x.Key != null)));
            }
        }

        return Task.FromResult<OpenIddictRequest?>(null);
    }
}
