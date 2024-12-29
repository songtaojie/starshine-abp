using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Starshine.Abp.Identity.Localization;
using Volo.Abp;

namespace Starshine.Abp.Identity
{
    /// <summary>
    /// 身份用户验证器
    /// </summary>
    public class StarshineIdentityUserValidator : IUserValidator<IdentityUser>
    {
        /// <summary>
        /// 本地化
        /// </summary>
        protected IStringLocalizer<IdentityResource> Localizer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="localizer"></param>
        public StarshineIdentityUserValidator(IStringLocalizer<IdentityResource> localizer)
        {
            Localizer = localizer;
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            var describer = new IdentityErrorDescriber();

            Check.NotNull(manager, nameof(manager));
            Check.NotNull(user, nameof(user));

            var errors = new List<IdentityError>();

            var userName = await manager.GetUserNameAsync(user);
            if (userName == null)
            {
                errors.Add(describer.InvalidUserName(null));
            }
            else
            {
                var owner = await manager.FindByEmailAsync(userName);
                if (owner != null && !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(new IdentityError
                    {
                        Code = "InvalidUserName",
                        Description = Localizer["InvalidUserName", userName]
                    });
                }
            }

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }
    }
}
