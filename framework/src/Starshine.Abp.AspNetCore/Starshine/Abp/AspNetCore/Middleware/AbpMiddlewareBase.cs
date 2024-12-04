// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.Controllers;
using Starshine.Abp.Core;
using System.Reflection;


namespace Starshine.Abp.AspNetCore.Middleware;

public abstract class StarshineMiddlewareBase : IMiddleware
{
    protected virtual Task<bool> ShouldSkipAsync(HttpContext context, RequestDelegate next)
    {
        var endpoint = context.GetEndpoint();
        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
        var disableAbpFeaturesAttribute = controllerActionDescriptor?.ControllerTypeInfo.GetCustomAttribute<DisableAbpFeaturesAttribute>();
        return Task.FromResult(disableAbpFeaturesAttribute != null && disableAbpFeaturesAttribute.DisableMiddleware);
    }

    public abstract Task InvokeAsync(HttpContext context, RequestDelegate next);
}
