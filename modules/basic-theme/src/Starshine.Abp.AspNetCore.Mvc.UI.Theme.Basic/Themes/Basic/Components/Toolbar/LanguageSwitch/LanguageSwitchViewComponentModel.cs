using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Toolbar.LanguageSwitch;

public class LanguageSwitchViewComponentModel
{
    public required LanguageInfo CurrentLanguage { get; set; }

    public required List<LanguageInfo> OtherLanguages { get; set; }
}
