﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Volo.Abp.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Starshine.Abp.Account.Web.ProfileManagement;
using Volo.Abp.Validation;

namespace Starshine.Abp.Account.Web.Pages.Account;

public class ManageModel : AccountPageModel
{
    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public ProfileManagementPageCreationContext ProfileManagementPageCreationContext { get; private set; }

    protected ProfileManagementPageOptions Options { get; }

    public ManageModel(IOptions<ProfileManagementPageOptions> options)
    {
        Options = options.Value;
        ProfileManagementPageCreationContext = default!;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ProfileManagementPageCreationContext = new ProfileManagementPageCreationContext(LazyServiceProvider);

        foreach (var contributor in Options.Contributors)
        {
            await contributor.ConfigureAsync(ProfileManagementPageCreationContext);
        }

        if (ReturnUrl != null)
        {
            if (!Url.IsLocalUrl(ReturnUrl) &&
                !ReturnUrl.StartsWith(UriHelper.BuildAbsolute(Request.Scheme, Request.Host, Request.PathBase).RemovePostFix("/")) &&
                !await AppUrlProvider.IsRedirectAllowedUrlAsync(ReturnUrl))
            {
                ReturnUrl = null;
            }
        }

        return Page();
    }

    public virtual Task<IActionResult> OnPostAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }
}
