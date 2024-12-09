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
/// 定义此接口是为了标准化请求分页结果。
/// </summary>
public interface IPagedRequest: ILimitedRequest
{
    /// <summary>
    /// 页码
    /// </summary>
    int Page { get; set; }
}
