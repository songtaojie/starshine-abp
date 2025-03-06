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
using Starshine.Abp.Application.Dtos;

namespace Starshine.Abp.Application.Dtos;
/// <summary>
/// <see cref="IPagedAndSortedResultRequest"/>.
/// </summary>
[Serializable]
public class PagedAndSortedRequestDto : PagedRequestDto, IPagedAndSortedRequest
{
    /// <summary>
    /// 排序字段
    /// </summary>
    public string? SortField { get; set; }

    /// <summary>
    /// 排序方向
    /// </summary>
    public SortDirectionEnum? SortDirection { get; set; } = SortDirectionEnum.ASC;
}
