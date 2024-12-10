// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Application;

/// <summary>
/// StarshineDddApplication模块
/// </summary>
[DependsOn(
    typeof(AbpDddApplicationModule)
    )]
public class StarshineDddApplicationModule : AbpModule
{

}
