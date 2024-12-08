// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Validation;

namespace Starshine.Abp.AspNetCore.UnifyResult;


/// <summary>
/// 规范化结果提供器
/// </summary>
public interface IUnifyResultHandler
{
    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    IActionResult OnSucceeded(ActionExecutedContext context, object? data);

    /// <summary>
    /// 验证失败返回值
    /// </summary>
    /// <param name="context"></param>
    /// <param name="validationResult"></param>
    /// <param name="statusCode">异常码</param>
    /// <returns></returns>
    IActionResult OnValidateFailed(ActionExecutingContext context, IAbpValidationResult validationResult,int statusCode);
}