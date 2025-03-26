// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp;

namespace Starshine.Abp.Swashbuckle;
[Area("Abp")]
[Route("Abp/Swashbuckle/[action]")]
[DisableAuditing]
[RemoteService(false)]
[ApiExplorerSettings(IgnoreApi = true)]
public class AbpSwashbuckleController : AbpController
{
    protected readonly IAbpAntiForgeryManager AntiForgeryManager;

    public AbpSwashbuckleController(IAbpAntiForgeryManager antiForgeryManager)
    {
        AntiForgeryManager = antiForgeryManager;
    }

    [HttpGet]
    public virtual void SetCsrfCookie()
    {
        AntiForgeryManager.SetCookie();
    }
}
