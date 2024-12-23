using System.Collections.Generic;

namespace Starshine.Abp.PermissionManagement;

public class GetPermissionListResultDto
{
    public string EntityDisplayName { get; set; }

    public List<PermissionGroupDto> Groups { get; set; }
}
