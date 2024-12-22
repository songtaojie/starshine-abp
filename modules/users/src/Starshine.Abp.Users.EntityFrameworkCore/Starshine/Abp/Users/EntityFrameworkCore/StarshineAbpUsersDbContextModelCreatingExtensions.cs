using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Starshine.Abp.Users.EntityFrameworkCore;

/// <summary>
/// 用户模型创建扩展
/// </summary>
public static class StarshineAbpUsersDbContextModelCreatingExtensions
{
    /// <summary>
    /// 配置用户实体
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <param name="b"></param>
    public static void ConfigureStarshineAbpUser<TUser>(this EntityTypeBuilder<TUser> b)
        where TUser : class, IUser
    {
        b.Property(u => u.TenantId).HasColumnName(nameof(IUser.TenantId));
        b.Property(u => u.UserName).IsRequired().HasMaxLength(StarshineAbpUserConsts.MaxUserNameLength).HasColumnName(nameof(IUser.UserName));
        b.Property(u => u.Email).IsRequired().HasMaxLength(StarshineAbpUserConsts.MaxEmailLength).HasColumnName(nameof(IUser.Email));
        b.Property(u => u.Name).HasMaxLength(StarshineAbpUserConsts.MaxNameLength).HasColumnName(nameof(IUser.Name));
        b.Property(u => u.Surname).HasMaxLength(StarshineAbpUserConsts.MaxSurnameLength).HasColumnName(nameof(IUser.Surname));
        b.Property(u => u.EmailConfirmed).HasDefaultValue(false).HasColumnName(nameof(IUser.EmailConfirmed));
        b.Property(u => u.PhoneNumber).HasMaxLength(StarshineAbpUserConsts.MaxPhoneNumberLength).HasColumnName(nameof(IUser.PhoneNumber));
        b.Property(u => u.PhoneNumberConfirmed).HasDefaultValue(false).HasColumnName(nameof(IUser.PhoneNumberConfirmed));
        b.Property(u => u.IsActive).HasColumnName(nameof(IUser.IsActive));
    }
}
