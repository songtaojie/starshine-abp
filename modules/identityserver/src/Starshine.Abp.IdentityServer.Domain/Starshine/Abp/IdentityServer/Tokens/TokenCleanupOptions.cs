using System;
using Volo.Abp.BackgroundWorkers;

namespace Starshine.Abp.IdentityServer.Tokens;

/// <summary>
///令牌清理选项
/// </summary>
public class TokenCleanupOptions
{
    /// <summary>
    /// Default: 3,600,000 ms.
    /// </summary>
    public int CleanupPeriod { get; set; } = 3_600_000;

    /// <summary>
    /// Default value: true.
    /// If <see cref="AbpBackgroundWorkerOptions.IsEnabled"/> is false,
    /// 此属性被忽略并且清理工作者不适用于该应用程序实例。
    /// </summary>
    public bool IsCleanupEnabled { get; set; } = true;
}
