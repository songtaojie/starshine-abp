﻿using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch;
using Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.UserMenu;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.Localization;
using Volo.Abp.Users;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Toolbars;

public class BasicThemeMainTopToolbarContributor : IToolbarContributor
{
    public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        if (context.Toolbar.Name != StandardToolbars.Main)
        {
            return;
        }

        if (context.Theme is not BasicTheme)
        {
            return;
        }

        var languageProvider = context.ServiceProvider.GetRequiredService<ILanguageProvider>();

        //TODO: This duplicates GetLanguages() usage. Can we eleminate this?
        var languages = await languageProvider.GetLanguagesAsync();
        if (languages.Count > 1)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(LanguageSwitchViewComponent)));
        }

        if (context.ServiceProvider.GetRequiredService<ICurrentUser>().IsAuthenticated)
        {
            context.Toolbar.Items.Add(new ToolbarItem(typeof(UserMenuViewComponent)));
        }
    }
}
