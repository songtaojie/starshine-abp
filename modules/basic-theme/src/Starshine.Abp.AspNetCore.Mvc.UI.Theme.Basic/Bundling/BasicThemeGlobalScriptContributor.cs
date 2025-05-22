using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQuery;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.JQueryValidationUnobtrusive;
using Volo.Abp.Modularity;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;

[DependsOn(
    typeof(JQueryScriptContributor),
    typeof(BootstrapScriptContributor),
    typeof(JQueryValidationUnobtrusiveScriptContributor)
    )]

public class BasicThemeGlobalScriptContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/themes/basic/layout.js");
    }
}
