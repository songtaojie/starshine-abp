using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.Account;
using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Identity;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Starshine.Abp.Account.Web.Pages.Account;

public abstract class AccountPageModel : AbpPageModel
{
    protected IAccountAppService AccountAppService  =>
         LazyServiceProvider.LazyGetRequiredService<IAccountAppService>();
    protected SignInManager<IdentityUser> SignInManager =>
            LazyServiceProvider.LazyGetRequiredService<SignInManager<IdentityUser>>();
    protected IdentityUserManager UserManager =>
            LazyServiceProvider.LazyGetRequiredService<IdentityUserManager>();
    protected IdentitySecurityLogManager IdentitySecurityLogManager =>
            LazyServiceProvider.LazyGetRequiredService<IdentitySecurityLogManager>();
    protected IOptions<IdentityOptions> IdentityOptions =>
            LazyServiceProvider.LazyGetRequiredService<IOptions<IdentityOptions>>();
    protected IExceptionToErrorInfoConverter ExceptionToErrorInfoConverter =>
            LazyServiceProvider.LazyGetRequiredService<IExceptionToErrorInfoConverter>();

    protected AccountPageModel()
    {
        LocalizationResourceType = typeof(AccountResource);
        ObjectMapperContext = typeof(StarshineAccountWebModule);
    }

    protected virtual void CheckCurrentTenant(Guid? tenantId)
    {
        if (CurrentTenant.Id != tenantId)
        {
            throw new ApplicationException($"Current tenant is different than given tenant. CurrentTenant.Id: {CurrentTenant.Id}, given tenantId: {tenantId}");
        }
    }

    protected virtual void CheckIdentityErrors(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException("Operation failed: " + identityResult.Errors.Select(e => $"[{e.Code}] {e.Description}").JoinAsString(", "));
        }

        //identityResult.CheckErrors(LocalizationManager); //TODO: Get from old Abp
    }

    protected virtual string GetLocalizeExceptionMessage(Exception exception)
    {
        if (exception is ILocalizeErrorMessage || exception is IHasErrorCode)
        {
            return ExceptionToErrorInfoConverter.Convert(exception).Message ?? string.Empty;
        }

        return exception.Message;
    }
}
