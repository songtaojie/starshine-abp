// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Volo.Abp.Authorization;
using Volo.Abp.DependencyInjection;
using Volo.Abp;

namespace Starshine.Abp.AspNetCore.ExceptionHandling;

public class DefaultAuthorizationExceptionHandler : IAuthorizationExceptionHandler, ITransientDependency
{
    public virtual async Task HandleAsync(AbpAuthorizationException exception, HttpContext httpContext)
    {
        var handlerOptions = httpContext.RequestServices.GetRequiredService<IOptions<AuthorizationExceptionHandlerOptions>>().Value;
        var isAuthenticated = httpContext.User.Identity?.IsAuthenticated ?? false;
        var authenticationSchemeProvider = httpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

        AuthenticationScheme? scheme = null;

        if (!handlerOptions.AuthenticationScheme.IsNullOrWhiteSpace())
        {
            scheme = await authenticationSchemeProvider.GetSchemeAsync(handlerOptions.AuthenticationScheme!);
            if (scheme == null)
            {
                throw new AbpException($"No authentication scheme named {handlerOptions.AuthenticationScheme} was found.");
            }
        }
        else
        {
            if (isAuthenticated)
            {
                scheme = await authenticationSchemeProvider.GetDefaultForbidSchemeAsync();
                if (scheme == null)
                {
                    throw new AbpException($"There was no DefaultForbidScheme found.");
                }
            }
            else
            {
                scheme = await authenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
                if (scheme == null)
                {
                    throw new AbpException($"There was no DefaultChallengeScheme found.");
                }
            }
        }

        var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name);
        if (handler == null)
        {
            throw new AbpException($"No handler of {scheme.Name} was found.");
        }

        if (isAuthenticated)
        {
            await handler.ForbidAsync(null);
        }
        else
        {
            await handler.ChallengeAsync(null);
        }
    }
}