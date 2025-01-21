namespace Starshine.Abp.PermissionManagement;

/// <summary>
/// 
/// </summary>
public class UpdatePermissionDto
{
    /// <summary>
    /// 
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsGranted { get; set; }
}
