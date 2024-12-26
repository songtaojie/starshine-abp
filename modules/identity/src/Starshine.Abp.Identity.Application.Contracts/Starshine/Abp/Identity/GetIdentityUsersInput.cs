using Volo.Abp.Application.Dtos;

namespace Starshine.Abp.Identity;

public class GetIdentityUsersInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
