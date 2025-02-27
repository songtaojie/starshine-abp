// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Http;

namespace Starshine.Abp.AspNetCore;

/// <summary>
/// 规范化结果响应
/// </summary>
public abstract class RESTfulResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// 执行成功
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    /// 异常码
    /// </summary>
    public object? ErrorCode { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public object? Errors { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// 成功
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static RESTfulResult<T> Successed<T>(T data)
    {
        return new RESTfulResult<T>
        {
            StatusCode = 200,
            ErrorCode = null,
            Succeeded = true,
            Data = data,
            Errors = null,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
    }

    /// <summary>
    /// 失败
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="errorCode"></param>
    /// <param name="errors"></param>
    /// <param name="statusCode"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static RESTfulResult<T> Failed<T>(string? errorCode,object? errors,int? statusCode = StatusCodes.Status400BadRequest, T? data = default)
    {
        return new RESTfulResult<T>
        {
            StatusCode = statusCode,
            ErrorCode = errorCode,
            Succeeded = false,
            Data = data,
            Errors = errors,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        };
    }
}
/// <summary>
/// RESTful 风格结果集,泛型结果集
/// </summary>
/// <typeparam name="T"></typeparam>
public class RESTfulResult<T>: RESTfulResult
{
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    
}