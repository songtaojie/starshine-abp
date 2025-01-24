using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starshine.Abp.Users.EntityFrameworkCore;

/// <summary>
/// 用户模型创建扩展
/// </summary>
public static class StarshineUsersDbContextModelCreatingExtensions
{
    /// <summary>
    /// 配置用户实体
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="b"></param>
    public static void ConfigureStarshineAbpUser<TUser>(this EntityTypeBuilder<TUser> b)
        where TUser : class, IUser
    {
        b.Property(u => u.TenantId);
        b.Property(u => u.UserName).IsRequired().HasMaxLength(StarshineUserConsts.MaxUserNameLength);
        b.Property(u => u.Email).IsRequired().HasMaxLength(StarshineUserConsts.MaxEmailLength);
        b.Property(u => u.Name).HasMaxLength(StarshineUserConsts.MaxNameLength);
        b.Property(u => u.Surname).HasMaxLength(StarshineUserConsts.MaxSurnameLength);
        b.Property(u => u.EmailConfirmed).HasDefaultValue(false);
        b.Property(u => u.PhoneNumber).HasMaxLength(StarshineUserConsts.MaxPhoneNumberLength);
        b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false);
        b.Property(u => u.IsActive);
    }
}
