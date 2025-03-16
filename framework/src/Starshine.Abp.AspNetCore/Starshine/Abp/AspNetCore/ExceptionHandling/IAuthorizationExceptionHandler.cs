// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Volo.Abp.Authorization;

namespace Starshine.Abp.AspNetCore.ExceptionHandling;

public interface IAuthorizationExceptionHandler
{
    Task HandleAsync(AbpAuthorizationException exception, HttpContext httpContext);
}
