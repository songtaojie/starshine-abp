// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Volo.Abp.Validation;

namespace Starshine.Abp.AspNetCore.UnifyResult;

/// <summary>
/// 默认的规范化结果提供器
/// </summary>
public class DefaultUnifyResultHandler : IUnifyResultHandler
{
    /// <summary>
    /// 响应成功返回结果
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public IActionResult OnSucceeded(ActionExecutedContext context, object? data)
    {
        return new JsonResult(RESTfulResult(StatusCodes.Status200OK,true, errorCode: 0, data));
    }

    /// <summary>
    /// 验证失败的结果
    /// </summary>
    /// <param name="context"></param>
    /// <param name="validationResult"></param>
    /// <param name="statusCode"></param>
    /// <returns></returns>
    public IActionResult OnValidateFailed(ActionExecutingContext context, IAbpValidationResult validationResult, int statusCode)
    {
        var errors = validationResult.Errors.ToDictionary(r=>r.MemberNames.First(), r=>r.ErrorMessage);
        return new JsonResult(RESTfulResult(statusCode, errorCode: statusCode, errors: errors));
    }

    /// <summary>
    /// 返回 RESTful 风格结果集
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="succeeded"></param>
    /// <param name="errorCode">业务错误代码</param>
    /// <param name="data"></param>
    /// <param name="errors"></param>
    /// <returns></returns>
    public RESTfulResult<object> RESTfulResult(int statusCode, bool succeeded = default,object? errorCode = default, object? data = default, object? errors = default)
    {
        return new RESTfulResult<object>
        {
            StatusCode = statusCode,
            ErrorCode = errorCode,
            Succeeded = succeeded,
            Data = data,
            Errors = errors,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
    }
}
