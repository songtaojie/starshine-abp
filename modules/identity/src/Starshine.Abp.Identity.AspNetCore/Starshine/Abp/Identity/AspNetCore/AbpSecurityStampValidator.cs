﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity.AspNetCore;

public class AbpSecurityStampValidator : SecurityStampValidator<IdentityUser>
{
    protected ITenantConfigurationProvider TenantConfigurationProvider { get; }
    protected ICurrentTenant CurrentTenant { get; }

    public AbpSecurityStampValidator(
        IOptions<SecurityStampValidatorOptions> options,
        SignInManager<IdentityUser> signInManager,
        ILoggerFactory loggerFactory,
        ITenantConfigurationProvider tenantConfigurationProvider,
        ICurrentTenant currentTenant)
        : base(
            options,
            signInManager,
            loggerFactory)
    {
        TenantConfigurationProvider = tenantConfigurationProvider;
        CurrentTenant = currentTenant;
    }

    [UnitOfWork]
    public async override Task ValidateAsync(CookieValidatePrincipalContext context)
    {
        TenantConfiguration tenant = null;
        try
        {
            tenant = await TenantConfigurationProvider.GetAsync(saveResolveResult: false);
        }
        catch (Exception e)
        {
            Logger.LogException(e);
        }

        using (CurrentTenant.Change(tenant?.Id, tenant?.Name))
        {
            await base.ValidateAsync(context);
        }
    }
}
