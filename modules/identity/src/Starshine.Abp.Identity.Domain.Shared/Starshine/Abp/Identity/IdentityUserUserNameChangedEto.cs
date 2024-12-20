using System;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Identity;

[Serializable]
public class IdentityUserUserNameChangedEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string UserName { get; set; }

    public string OldUserName { get; set; }
}
