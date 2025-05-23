﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Starshine.Abp.AspNetCore.Mvc.UI.Theme.Basic.Themes.Basic.Components.Brand;

public class MainNavbarBrandViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/Basic/Components/Brand/Default.cshtml");
    }
}
