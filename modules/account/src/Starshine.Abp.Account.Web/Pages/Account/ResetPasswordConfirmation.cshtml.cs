using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Starshine.Abp.Account.Web.Pages.Account;

[AllowAnonymous]
public class ResetPasswordConfirmationModel : AccountPageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        ReturnUrl = await GetRedirectUrlAsync(ReturnUrl, ReturnUrlHash);

        return Page();
    }
}
