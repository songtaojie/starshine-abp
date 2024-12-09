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
/// 定义此接口是为了标准化以请求有限的结果。
/// </summary>
public interface ILimitedRequest
{
    /// <summary>
    /// 应该返回最大结果计数。
    /// </summary>
    int PageSize { get; set; }
}
