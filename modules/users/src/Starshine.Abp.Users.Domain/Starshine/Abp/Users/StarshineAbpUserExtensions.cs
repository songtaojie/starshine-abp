namespace Starshine.Abp.Users;

/// <summary>
/// �û���չ��
/// </summary>
public static class StarshineAbpUserExtensions
{
    /// <summary>
    /// userתUserData
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static IUserData ToStarshineAbpUserData(this IUser user)
    {
        return new UserData(
            id: user.Id,
            userName: user.UserName,
            email: user.Email,
            name: user.Name,
            surname: user.Surname,
            isActive: user.IsActive,
            emailConfirmed: user.EmailConfirmed,
            phoneNumber: user.PhoneNumber,
            phoneNumberConfirmed: user.PhoneNumberConfirmed,
            tenantId: user.TenantId,
            extraProperties: user.ExtraProperties
        );
    }
}
