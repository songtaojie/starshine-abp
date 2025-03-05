namespace Starshine.Abp.Identity;

/// <summary>
/// 组织单位常量
/// </summary>
public static class OrganizationUnitConsts
{
    /// <summary>
    ///DisplayName 属性的最大长度。
    /// </summary>
    public static int MaxDisplayNameLength { get; set; } = 128;

    /// <summary>
    ///OU 层次结构的最大深度。
    /// </summary>
    public const int MaxDepth = 16;

    /// <summary>
    /// 点之间的代码单元的长度。
    /// </summary>
    public const int CodeUnitLength = 5;

    /// <summary>
    ///Code 属性的最大长度。
    /// </summary>
    public const int MaxCodeLength = MaxDepth * (CodeUnitLength + 1) - 1;
}
