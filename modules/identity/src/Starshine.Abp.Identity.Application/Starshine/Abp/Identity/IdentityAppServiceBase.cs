﻿using Volo.Abp.Application.Services;
using Starshine.Abp.Identity.Localization;

namespace Starshine.Abp.Identity;

public abstract class IdentityAppServiceBase : ApplicationService
{
    protected IdentityAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpIdentityApplicationModule);
        LocalizationResource = typeof(IdentityResource);
    }
}
