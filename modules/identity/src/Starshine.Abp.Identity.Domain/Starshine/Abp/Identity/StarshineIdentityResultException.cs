using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Volo.Abp.ExceptionHandling;
using Starshine.Abp.Identity.Localization;
using Volo.Abp.Localization;
using Volo.Abp;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份结果异常
/// </summary>
public class StarshineIdentityResultException : BusinessException, ILocalizeErrorMessage
{
    /// <summary>
    /// 身份结果
    /// </summary>
    public IdentityResult IdentityResult { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityResult"></param>
    public StarshineIdentityResultException([NotNull] IdentityResult identityResult)
        : base(code: $"Starshine.Abp.Identity:{identityResult.Errors.First().Code}",message: identityResult.Errors.Select(err => err.Description).JoinAsString(", "))
    {
        IdentityResult = Check.NotNull(identityResult, nameof(identityResult));
    }

    /// <summary>
    /// 本地化消息
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public virtual string LocalizeMessage(LocalizationContext context)
    {
        var localizer = context.LocalizerFactory.Create<IdentityResource>();

        SetData(localizer);

        return IdentityResult.LocalizeErrors(localizer);
    }

    /// <summary>
    /// 设置数据
    /// </summary>
    /// <param name="localizer"></param>
    protected virtual void SetData(IStringLocalizer localizer)
    {
        var values = IdentityResult.GetValuesFromErrorMessage(localizer);

        for (var index = 0; index < values.Length; index++)
        {
            Data[index.ToString()] = values[index];
        }
    }
}
