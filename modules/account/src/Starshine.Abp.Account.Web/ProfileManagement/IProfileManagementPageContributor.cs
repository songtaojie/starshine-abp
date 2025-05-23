﻿﻿using System.Threading.Tasks;

namespace Starshine.Abp.Account.Web.ProfileManagement;

public interface IProfileManagementPageContributor
{
    Task ConfigureAsync(ProfileManagementPageCreationContext context);
}
