// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Starshine.Abp.AspNetCore;

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