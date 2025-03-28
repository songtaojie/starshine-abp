﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Starshine.Abp.OpenIddict.ExtensionGrantTypes;

public interface IExtensionGrant
{
    string Name { get; }

    Task<IActionResult> HandleAsync(ExtensionGrantContext context);
}
