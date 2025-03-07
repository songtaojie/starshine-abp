using Starshine.Abp.Identity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starshine.Abp.Identity
{
    /// <summary>
    /// Dto扩展类
    /// </summary>
    public static class DtoExtensions
    {
        /// <summary>
        /// 转换为IdentityUserDto
        /// </summary>
        /// <param name="identityUser"></param>
        /// <returns></returns>
        public static IdentityUserDto ToIdentityUserDto(this IdentityUser identityUser)
        {
            return new IdentityUserDto
            {
                AccessFailedCount = identityUser.AccessFailedCount,
                ConcurrencyStamp = identityUser.ConcurrencyStamp,
                CreationTime = identityUser.CreationTime,
                CreatorId = identityUser.CreatorId,
                DeleterId = identityUser.DeleterId,
                DeletionTime = identityUser.DeletionTime,
                Email = identityUser.Email,
                EmailConfirmed = identityUser.EmailConfirmed,
                EntityVersion = identityUser.EntityVersion,
                Id = identityUser.Id,
                Name = identityUser.Name,
                IsActive = identityUser.IsActive,
                IsDeleted = identityUser.IsDeleted,
                LastModificationTime = identityUser.LastModificationTime,
                LastModifierId = identityUser.LastModifierId,
                LastPasswordChangeTime = identityUser.LastPasswordChangeTime,
                LockoutEnabled = identityUser.LockoutEnabled,
                LockoutEnd = identityUser.LockoutEnd,
                PhoneNumber = identityUser.PhoneNumber,
                PhoneNumberConfirmed = identityUser.PhoneNumberConfirmed,
                Surname = identityUser.Surname,
                TenantId = identityUser.TenantId,
                UserName = identityUser.UserName
            };
        }

        /// <summary>
        /// 转换为角色
        /// </summary>
        /// <param name="identityRole"></param>
        /// <returns></returns>
        public static IdentityRoleDto ToIdentityRoleDto(this IdentityRole identityRole)
        {
            return new IdentityRoleDto
            {
                IsStatic = identityRole.IsStatic,
                Id = identityRole.Id,
                Name = identityRole.Name,
                ConcurrencyStamp = identityRole.ConcurrencyStamp,
                IsDefault = identityRole.IsDefault,
                IsPublic = identityRole.IsPublic
            };
        }

    }
}
