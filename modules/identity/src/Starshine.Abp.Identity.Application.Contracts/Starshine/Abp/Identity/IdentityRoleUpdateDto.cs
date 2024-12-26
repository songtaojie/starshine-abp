using Volo.Abp.Domain.Entities;

namespace Starshine.Abp.Identity;

public class IdentityRoleUpdateDto : IdentityRoleCreateOrUpdateDtoBase, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
