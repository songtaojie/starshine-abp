using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity;

public class GetIdentityRolesInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
