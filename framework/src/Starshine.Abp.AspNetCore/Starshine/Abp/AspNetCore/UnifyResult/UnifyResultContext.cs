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
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Collections;
using Volo.Abp.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    /// <returns></returns>
    internal static bool IsSkipUnifyHandler(ActionContext context)
    {
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (actionDescriptor == null || !IsEnabledUnifyHandle) return false;

        var method = actionDescriptor.MethodInfo;
        // 判断是否跳过规范化处理
        var isSkip = !method.GetRealReturnType().IsAssignableToGenericType(RESTfulResultType)
              || method.CustomAttributes.Any(x => typeof(NonUnifyAttribute).IsAssignableFrom(x.AttributeType) || typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType) || typeof(IApiResponseMetadataProvider).IsAssignableFrom(x.AttributeType))
              || (method.ReflectedType != null && method.ReflectedType.IsDefined(typeof(NonUnifyAttribute), true));

        return isSkip;
    }

    /// <summary>
    /// 获取验证错误信息
    /// </summary>
    /// <param name="errors"></param>
    /// <returns></returns>
    internal static IAbpValidationResult GetAbpValidationResult(object? errors)
    {
        var validationResult = new AbpValidationResult();
        // 判断是否是集合类型
        if (errors is IEnumerable && errors is not string)
        {
            // 如果是模型验证字典类型
            if (errors is ModelStateDictionary modelState)
            {
                var validationResults = modelState.Where(u => modelState[u.Key]!.ValidationState == ModelValidationState.Invalid)
                    .SelectMany(u => u.Value!.Errors.Select(error => new ValidationResult(error.ErrorMessage, new string[] { u.Key })));
                validationResult.Errors.AddRange(validationResults);
            }
            // 如果是 ValidationProblemDetails 特殊类型
            else if (errors is ValidationProblemDetails validation)
            {
                var validationResults = validation.Errors.SelectMany(r => r.Value.Select(errorMessage => new ValidationResult(errorMessage, new[] { r.Key })));
                validationResult.Errors.AddRange(validationResults);
            }
            // 如果是字典类型
            else if (errors is Dictionary<string, string[]> dicResults)
            {
                var validationResults = dicResults.SelectMany(r => r.Value.Select(errorMessage => new ValidationResult(errorMessage, new[] { r.Key })));
                validationResult.Errors.AddRange(validationResults);
            }
        }
        // 其他类型
        else
        {
            validationResult.Errors.Add(new ValidationResult(errors?.ToString(), new string[] { "Default" }));
        }
        
        return validationResult;
    }

    /// <summary>
    /// 检查是否是有效的结果（可进行规范化的结果）
    /// </summary>
    /// <param name="result"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    internal static bool CheckVaildResult(IActionResult? result, out object? data)
    {
        data = default;

        // 排除以下结果，跳过规范化处理
        var isDataResult = result switch
        {
            ViewResult => false,
            PartialViewResult => false,
            FileResult => false,
            ChallengeResult => false,
            SignInResult => false,
            SignOutResult => false,
            RedirectToPageResult => false,
            RedirectToRouteResult => false,
            RedirectResult => false,
            RedirectToActionResult => false,
            LocalRedirectResult => false,
            ForbidResult => false,
            ViewComponentResult => false,
            PageResult => false,
            NotFoundResult => false,
            NotFoundObjectResult => false,
            _ => true,
        };

        // 目前支持返回值 ActionResult
        if (isDataResult) data = result switch
        {
            // 处理内容结果
            ContentResult content => content.Content,
            // 处理对象结果
            ObjectResult obj => obj.Value,
            // 处理 JSON 对象
            JsonResult json => json.Value,
            _ => null,
        };

        return isDataResult;
    }
}
