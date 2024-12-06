// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

namespace Starshine.Abp.AspNetCore;

/// <summary>
/// 异常元数据
/// </summary>
public sealed class ExceptionMetadata
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; internal set; }

    /// <summary>
    /// 错误码
    /// </summary>
    public object? ErrorCode { get; internal set; }

    /// <summary>
    /// 错误对象（信息）
    /// </summary>
    public object? Errors { get; internal set; }

    /// <summary>
    /// 错误对象（信息）
    /// </summary>
    public string? ErrorMessage { get; internal set; }

    /// <summary>
    /// 额外数据
    /// </summary>
    public object? Data { get; internal set; }
}
