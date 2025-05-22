using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.Core;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.FontAwesome;
using Volo.Abp.Modularity;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
[DependsOn(
    typeof(CoreStyleContributor),
    typeof(BootstrapStyleContributor)
)]
public class BasicThemeGlobalStyleContributor : BundleContributor
{
    public override void ConfigureBundle(BundleConfigurationContext context)
    {
        context.Files.AddIfNotContains("/themes/basic/layout.css");
        context.Files.AddIfNotContains("/libs/font-awesome/css/all.min.css");
        context.Files.AddIfNotContains("/libs/font-awesome/css/v4-shims.min.css");
    }
}
