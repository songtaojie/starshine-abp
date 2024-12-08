// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.Microsoft.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Volo.Abp.Json;
using Starshine.Abp.AspNetCore.UnifyResult;

namespace Starshine.Abp.AspNetCore;

/// <summary>
/// 规范化结构（请求成功）过滤器
/// </summary>
public class UnifyResultFilterAttribute : IAsyncActionFilter, IOrderedFilter
{
    /// <summary>
    /// 排序
    /// </summary>
    public int Order => 9999;

    /// <summary>
    /// 方法执行后
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 执行 Action 并获取结果
        var actionExecutedContext = await next();

        // 排除 WebSocket 请求处理
        if (actionExecutedContext.HttpContext.IsWebSocketRequest()) return;
        // 判断是否跳过规范化处理
        if (UnifyResultContext.IsSkipUnifyHandler(context)) return;

        // 处理已经含有状态码结果的 Result
        if (actionExecutedContext.Result is IStatusCodeActionResult statusCodeResult && statusCodeResult.StatusCode.HasValue)
        {
            // 小于 200 或者 大于 299 都不是成功值，直接跳过
            if (statusCodeResult.StatusCode.Value < 200 || statusCodeResult.StatusCode.Value > 299)
            {
                // 处理规范化结果
                var httpContext = context.HttpContext;
                var statusCode = statusCodeResult.StatusCode.Value;

                // 解决刷新 Token 时间和 Token 时间相近问题
                if (statusCodeResult.StatusCode.Value == StatusCodes.Status401Unauthorized
                    && httpContext.Response.Headers.ContainsKey("access-token")
                    && httpContext.Response.Headers.ContainsKey("x-access-token"))
                {
                    httpContext.Response.StatusCode = statusCode = StatusCodes.Status403Forbidden;
                }

                // 如果 Response 已经完成输出，则禁止写入
                if (httpContext.Response.HasStarted) return;
                var jsonSerializer = httpContext.RequestServices.GetRequiredService<IJsonSerializer>();
                httpContext.Response.Headers.Append("Content-Type", "application/json");
                await httpContext.Response.WriteAsync(jsonSerializer.Serialize(new RESTfulResult<object>
                {
                    StatusCode = statusCode,
                    ErrorCode = statusCode,
                    Succeeded = false,
                    Data = null,
                    Errors = "403 Forbidden",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                }));
                return;
            }
        }

        // 如果出现异常，则不会进入该过滤器
        if (actionExecutedContext.Exception != null) return;
        
        // 获取控制器信息
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor == null) return;
        if (typeof(Controller).IsAssignableFrom(actionDescriptor.ControllerTypeInfo)) return;
        // 处理 BadRequestObjectResult 类型规范化处理
        if (actionExecutedContext.Result is BadRequestObjectResult badRequestObjectResult)
        {
            // 解析验证消息
            var validationResult = UnifyResultContext.GetAbpValidationResult(badRequestObjectResult.Value);
            var unifyResultHandler = context.HttpContext.RequestServices.GetRequiredService<IUnifyResultHandler>();
            var statusCode = badRequestObjectResult.StatusCode ?? StatusCodes.Status400BadRequest;
            var result = unifyResultHandler.OnValidateFailed(context, validationResult, statusCode);
            if (result != null) actionExecutedContext.Result = result;
        }
        else
        {
            IActionResult? result = default;
            var unifyResultHandler = context.HttpContext.RequestServices.GetRequiredService<IUnifyResultHandler>();
            // 检查是否是有效的结果（可进行规范化的结果）
            if (UnifyResultContext.CheckVaildResult(actionExecutedContext.Result, out var data))
            {
                result = unifyResultHandler.OnSucceeded(actionExecutedContext, data);
            }
            // 如果是不能规范化的结果类型，则跳过
            if (result == null) return;
            actionExecutedContext.Result = result;
        }
    }
}