using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Starshine.Abp.Account.Web.Consts;
using Volo.Abp.Identity;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Validation;

namespace Starshine.Abp.Account.Web.Pages.Account;

public class ForgotPasswordModel : AccountPageModel
{
    [Required]
    [EmailAddress]
    [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
    [BindProperty]
    [DisplayName("邮箱")]
    public string? Email { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrlHash { get; set; }

    public virtual Task<IActionResult> OnGetAsync()
    {
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var AppUrlProvider = LazyServiceProvider.GetRequiredService<IAppUrlProvider>();
            var url = await AppUrlProvider.GetUrlAsync(StarshineAccountConsts.AppName, StarshineAccountConsts.PasswordReset);
            await AccountAppService.SendPasswordResetCodeAsync(
                new SendPasswordResetCodeDto
                {
                    Email = Email,
                    AppName = StarshineAccountConsts.AppName, //TODO: Const!
                    ReturnUrl = ReturnUrl,
                    ReturnUrlHash = ReturnUrlHash
                }
            );
        }
        catch (UserFriendlyException e)
        {
            Alerts.Danger(GetLocalizeExceptionMessage(e));
            return Page();
        }


        return RedirectToPage(
            "./PasswordResetLinkSent",
            new
            {
                returnUrl = ReturnUrl,
                returnUrlHash = ReturnUrlHash
            });
    }
}
