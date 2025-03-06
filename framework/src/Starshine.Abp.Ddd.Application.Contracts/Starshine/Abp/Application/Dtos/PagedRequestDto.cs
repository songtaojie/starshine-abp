// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Abp.Application.Dtos;

namespace Starshine.Abp.Application.Dtos;
/// <summary>
/// 分页请求
/// </summary>
[Serializable]
public class PagedRequestDto : LimitedRequestDto, IPagedRequest
{
    /// <summary>
    /// 页码
    /// </summary>
    [Range(0, int.MaxValue)]
    public virtual int Page { get; set; }
}
