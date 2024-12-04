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

namespace Starshine.Abp.Core;
[AttributeUsage(AttributeTargets.Class)]
public class DisableAbpFeaturesAttribute : Attribute
{
    /// <summary>
    /// The framework will not register any interceptors for the class.
    /// This will cause the all features that depend on interceptors to not work.
    /// </summary>
    public bool DisableInterceptors { get; set; } = true;

    /// <summary>
    /// The framework middleware will skip the class.
    /// This will cause the all features that depend on middleware to not work.
    /// </summary>
    public bool DisableMiddleware { get; set; } = true;

    /// <summary>
    /// The framework will not remove all built-in filters for the class.
    /// This will cause the all features that depend on filters to not work.
    /// </summary>
    public bool DisableMvcFilters { get; set; } = true;
}
