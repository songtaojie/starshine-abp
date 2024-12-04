// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Components.Endpoints;
using Microsoft.Extensions.Options;
using Starshine.Abp.AspNetCore.Middleware;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Starshine.Abp.AspNetCore.Uow;

public class StarshineUnitOfWorkMiddleware : StarshineMiddlewareBase, ITransientDependency
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public StarshineUnitOfWorkMiddleware(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    public async override Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (await ShouldSkipAsync(context, next))
        {
            await next(context);
            return;
        }

        using var uow = _unitOfWorkManager.Reserve(UnitOfWork.UnitOfWorkReservationName);
        await next(context);
        await uow.CompleteAsync(context.RequestAborted);
    }

    protected async override Task<bool> ShouldSkipAsync(HttpContext context, RequestDelegate next)
    {
        // Blazor components will render concurrently, so we need to skip the middleware for them.
        // Otherwise, We will get the following exception:
        // A second operation started on this context before a previous operation completed.
        // This is usually caused by different threads using the same instance of DbContext.
        if (context.GetEndpoint()?.Metadata?.GetMetadata<RootComponentMetadata>() != null)
        {
            return true;
        }

        return await base.ShouldSkipAsync(context, next);
    }
}
