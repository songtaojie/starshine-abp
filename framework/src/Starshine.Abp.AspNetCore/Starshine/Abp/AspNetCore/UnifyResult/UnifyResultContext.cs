// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using System.Reflection;

namespace Starshine.Abp.AspNetCore;

/// <summary>
/// 规范化结果上下文
/// </summary>
public static class UnifyResultContext
{
    /// <summary>
    /// 是否启用规范化结果
    /// </summary>
    internal static bool IsEnabledUnifyHandle = false;

    /// <summary>
    /// 规范化结果类型
    /// </summary>
    internal static Type? RESTfulResultType = typeof(RESTfulResult<>);

    /// <summary>
    /// 获取异常元数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ExceptionMetadata GetExceptionMetadata(ActionContext context)
    {
        // 获取错误码
        object? errorCode = default;
        object? errors = default;
        string? errorMessage = null;
        object? data = default;
        var statusCode = StatusCodes.Status500InternalServerError;
        // 判断是否是 ExceptionContext 或者 ActionExecutedContext
        var exception = context is ExceptionContext exContext
            ? exContext.Exception
            : (
                context is ActionExecutedContext edContext
                ? edContext.Exception
                : default
            );
        if (exception is UserFriendlyException friendlyException)
        {
            errorCode = friendlyException.Code;
            errors = friendlyException.Details;
            data = friendlyException.Data;
            errorMessage = friendlyException.Message;
        }
        else if (exception != null)
        {
            errorMessage = exception.Message;
            errors = exception.Message;
        }

        return new ExceptionMetadata
        {
            Data = data,
            ErrorCode = errorCode,
            Errors = errors,
            StatusCode = statusCode,
            ErrorMessage = errorMessage
        };
    }

    /// <summary>
    /// 是否跳过成功返回结果规范处理（状态码 200~209 ）
    /// </summary>
    /// <param name="context"></param>
    /// <param name="isWebRequest"></param>
    /// <returns></returns>
    internal static bool IsSkipUnifyHandler(ActionContext context)
    {
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor == null || !IsEnabledUnifyHandle)
        {
            return false;
        }
        var method = actionDescriptor.MethodInfo;
        // 判断是否跳过规范化处理
        var isSkip = !method.GetRealReturnType().IsAssignableToGenericType(RESTfulResultType)
              || method.CustomAttributes.Any(x => typeof(NonUnifyAttribute).IsAssignableFrom(x.AttributeType) || typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType) || typeof(IApiResponseMetadataProvider).IsAssignableFrom(x.AttributeType))
              || (method.ReflectedType != null && method.ReflectedType.IsDefined(typeof(NonUnifyAttribute), true));

        return isSkip;
    }
}
