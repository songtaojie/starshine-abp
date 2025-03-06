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
/// 此接口定义用于标准化对分页和排序结果的请求。
/// </summary>
public interface IPagedAndSortedRequest : IPagedRequest, ISortedRequest
{

}