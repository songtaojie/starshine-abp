﻿using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Bundling;
using Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic;

[DependsOn(
    typeof(AbpAspNetCoreMvcUiThemeSharedModule),
    typeof(AbpAspNetCoreMvcUiMultiTenancyModule)
    )]
public class StarshineAspNetCoreMvcUIBasicThemeModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(StarshineAspNetCoreMvcUIBasicThemeModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpThemingOptions>(options =>
        {
            options.Themes.Add<BasicTheme>();

            options.DefaultThemeName ??= BasicTheme.Name;
        });

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<StarshineAspNetCoreMvcUIBasicThemeModule>("Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic");
        });

        Configure<AbpToolbarOptions>(options =>
        {
            options.Contributors.Add(new BasicThemeMainTopToolbarContributor());
        });

        Configure<AbpBundlingOptions>(options =>
        {
            options
                .StyleBundles
                .Add(BasicThemeBundles.Styles.Global, bundle =>
                {
                    bundle
                        //.AddBaseBundles(StandardBundles.Styles.Global)
                        .AddContributors(typeof(BasicThemeGlobalStyleContributor));
                });

            options
                .ScriptBundles
                .Add(BasicThemeBundles.Scripts.Global, bundle =>
                {
                    bundle
                        //.AddBaseBundles(StandardBundles.Scripts.Global)
                        .AddContributors(typeof(BasicThemeGlobalScriptContributor));
                });
        });
    }
}
