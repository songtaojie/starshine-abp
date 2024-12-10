// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Application.Dtos;
/// <summary>
/// 排序
/// </summary>
public interface ISortedRequest
{
    /// <summary>
    /// 排序的字段
    /// </summary>
    string? SortField { get; set; }

    /// <summary>
    /// 0 正序 1倒序
    /// </summary>
    SortDirectionEnum? SortDirection { get; set; }
}

/// <summary>
/// 排序类型
/// </summary>
public enum SortDirectionEnum
{
    /// <summary>
    /// 正序
    /// </summary>
    ASC = 0,
    /// <summary>
    /// 倒序
    /// </summary>
    DESC = 1
}
