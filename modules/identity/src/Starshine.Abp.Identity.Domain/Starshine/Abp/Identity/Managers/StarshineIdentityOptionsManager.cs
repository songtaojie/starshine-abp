using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Starshine.Abp.Identity.Settings;
using Volo.Abp.Options;
using Volo.Abp.Settings;

namespace Starshine.Abp.Identity.Managers;

/// <summary>
/// 身份选项管理器
/// </summary>
public class StarshineIdentityOptionsManager : AbpDynamicOptionsManager<IdentityOptions>
{
    /// <summary>
    /// 设置提供者
    /// </summary>
    protected ISettingProvider SettingProvider { get; }

    /// <summary>
    /// 身份选项管理器
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="settingProvider"></param>
    public StarshineIdentityOptionsManager(IOptionsFactory<IdentityOptions> factory,ISettingProvider settingProvider) : base(factory)
    {
        SettingProvider = settingProvider;
    }

    /// <summary>
    /// 覆盖选项
    /// </summary>
    /// <param name="name"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    protected override async Task OverrideOptionsAsync(string name, IdentityOptions options)
    {
        options.Password.RequiredLength = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequiredLength, options.Password.RequiredLength);
        options.Password.RequiredUniqueChars = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequiredUniqueChars, options.Password.RequiredUniqueChars);
        options.Password.RequireNonAlphanumeric = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, options.Password.RequireNonAlphanumeric);
        options.Password.RequireLowercase = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireLowercase, options.Password.RequireLowercase);
        options.Password.RequireUppercase = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireUppercase, options.Password.RequireUppercase);
        options.Password.RequireDigit = await SettingProvider.GetAsync(IdentitySettingNames.Password.RequireDigit, options.Password.RequireDigit);

        options.Lockout.AllowedForNewUsers = await SettingProvider.GetAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, options.Lockout.AllowedForNewUsers);
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(await SettingProvider.GetAsync(IdentitySettingNames.Lockout.LockoutDuration, options.Lockout.DefaultLockoutTimeSpan.TotalSeconds.To<int>()));
        options.Lockout.MaxFailedAccessAttempts = await SettingProvider.GetAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, options.Lockout.MaxFailedAccessAttempts);

        options.SignIn.RequireConfirmedEmail = await SettingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, options.SignIn.RequireConfirmedEmail);
        options.SignIn.RequireConfirmedPhoneNumber = await SettingProvider.GetAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, options.SignIn.RequireConfirmedPhoneNumber);
    }
}
