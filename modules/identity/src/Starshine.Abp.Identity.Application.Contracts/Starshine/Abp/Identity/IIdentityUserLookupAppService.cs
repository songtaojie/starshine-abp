﻿using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Starshine.Abp.Identity;

[Obsolete("Use IIdentityUserIntegrationService for module-to-module (or service-to-service) communication.")]
public interface IIdentityUserLookupAppService : IApplicationService
{
    Task<UserData> FindByIdAsync(Guid id);

    Task<UserData> FindByUserNameAsync(string userName);

    Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input);

    Task<long> GetCountAsync(UserLookupCountInputDto input);
}