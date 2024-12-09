// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Volo.Abp.Application;
using Volo.Abp.Auditing;
using Volo.Abp.Data;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Starshine.Abp.Ddd.Application.Contracts;
/// <summary>
/// Starshine Ddd模块
/// </summary>
[DependsOn(
    typeof(AbpDddApplicationContractsModule)
    )]
public class StarshineDddApplicationContractsModule : AbpModule
{

}
