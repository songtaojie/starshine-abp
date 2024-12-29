using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Localization;
using Volo.Abp;
using Starshine.Abp.Identity;
using Volo.Abp.Text.Formatting;

namespace Microsoft.AspNetCore.Identity;

/// <summary>
/// 
/// </summary>
public static class StarshineIdentityResultExtensions
{
    private static readonly Dictionary<string, string> IdentityStrings = new Dictionary<string, string>();

    static StarshineIdentityResultExtensions()
    {
        var identityResourceManager = new ResourceManager("Microsoft.Extensions.Identity.Core.Resources", typeof(UserManager<>).Assembly);
        var resourceSet = identityResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, false);
        if (resourceSet == null)
        {
            throw new AbpException("Can't get the ResourceSet of Identity.");
        }

        var iterator = resourceSet.GetEnumerator();
        while (true)
        {
            if (!iterator.MoveNext())
            {
                break;
            }

            var key = iterator.Key?.ToString();
            var value = iterator.Value?.ToString();
            if (key != null && value != null)
            {
                IdentityStrings.Add(key, value);
            }
        }

        if (!IdentityStrings.Any())
        {
            throw new AbpException("ResourceSet values of Identity is empty.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityResult"></param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="StarshineIdentityResultException"></exception>
    public static void CheckErrors(this IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
        {
            return;
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        throw new StarshineIdentityResultException(identityResult);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityResult"></param>
    /// <param name="localizer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string[] GetValuesFromErrorMessage(this IdentityResult identityResult, IStringLocalizer localizer)
    {
        if (identityResult.Succeeded)
        {
            throw new ArgumentException(
                "identityResult.Succeeded should be false in order to get values from error.");
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        var error = identityResult.Errors.First();
        var englishString = IdentityStrings.GetOrDefault(error.Code);

        if (englishString == null)
        {
            return Array.Empty<string>();
        }

        if (FormattedStringValueExtracter.IsMatch(error.Description, englishString, out var values))
        {
            return values;
        }

        return Array.Empty<string>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="identityResult"></param>
    /// <param name="localizer"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string LocalizeErrors(this IdentityResult identityResult, IStringLocalizer localizer)
    {
        if (identityResult.Succeeded)
        {
            throw new ArgumentException("identityResult.Succeeded should be false in order to localize errors.");
        }

        if (identityResult.Errors == null)
        {
            throw new ArgumentException("identityResult.Errors should not be null.");
        }

        return identityResult.Errors.Select(err => LocalizeErrorMessage(err, localizer)).JoinAsString(", ");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="error"></param>
    /// <param name="localizer"></param>
    /// <returns></returns>
    public static string LocalizeErrorMessage(this IdentityError error, IStringLocalizer localizer)
    {
        var key = $"Starshine.Abp.Identity:{error.Code}";

        var localizedString = localizer[key];

        if (!localizedString.ResourceNotFound)
        {
            var englishString = IdentityStrings.GetOrDefault(error.Code);
            if (englishString != null)
            {
                if (FormattedStringValueExtracter.IsMatch(error.Description, englishString, out var values))
                {
                    return string.Format(localizedString.Value, values.Cast<object>().ToArray());
                }
            }
        }

        return localizer["Starshine.Abp.Identity:DefaultError"];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="signInResult"></param>
    /// <returns></returns>
    public static string GetResultAsString(this SignInResult signInResult)
    {
        if (signInResult.Succeeded)
        {
            return "Succeeded";
        }

        if (signInResult.IsLockedOut)
        {
            return "IsLockedOut";
        }

        if (signInResult.IsNotAllowed)
        {
            return "IsNotAllowed";
        }

        if (signInResult.RequiresTwoFactor)
        {
            return "RequiresTwoFactor";
        }

        return "Unknown";
    }
}
