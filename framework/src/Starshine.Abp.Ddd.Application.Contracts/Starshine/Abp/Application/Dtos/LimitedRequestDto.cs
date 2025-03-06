// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starshine.Abp.Application.Dtos;
using Starshine.Abp.Application.Localization.Resources.AbpDdd;

namespace Starshine.Abp.Application.Dtos;

/// <summary>
///实现 <see cref="ILimitedRequest"/>.
/// </summary>
[Serializable]
public class LimitedRequestDto : ILimitedRequest, IValidatableObject
{
    /// <summary>
    /// Default value: 10.
    /// </summary>
    public static int DefaultPageSize { get; set; } = 10;

    /// <summary>
    /// 最大可能值 <see cref="PageSize"/>.
    /// Default value: 1,000.
    /// </summary>
    public static int MaxPageSize { get; set; } = 1000;

    /// <summary>
    /// Maximum result count should be returned.
    /// This is generally used to limit result count on paging.
    /// </summary>
    [Range(1, int.MaxValue)]
    public virtual int PageSize { get; set; } = DefaultPageSize;

    /// <summary>
    /// 验证
    /// </summary>
    /// <param name="validationContext"></param>
    /// <returns></returns>
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PageSize > MaxPageSize)
        {
            var localizer = validationContext.GetRequiredService<IStringLocalizer<StarshineDddApplicationContractsResource>>();

            yield return new ValidationResult(
                localizer[
                    "MaxResultCountExceededExceptionMessage",
                    nameof(PageSize),
                    MaxPageSize,
                    typeof(LimitedResultRequestDto).FullName!,
                    nameof(MaxPageSize)
                ],
                new[] { nameof(PageSize) });
        }
    }
}