using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace Starshine.Abp.IdentityServer.Tokens;
/// <summary>
/// 令牌清理后台工作者
/// </summary>
public class TokenCleanupBackgroundWorker : AsyncPeriodicBackgroundWorkerBase
{
    /// <summary>
    /// 令牌清理选项
    /// </summary>
    protected TokenCleanupOptions Options { get; }

    /// <summary>
    /// 令牌清理后台工作者
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="serviceScopeFactory"></param>
    /// <param name="options"></param>
    public TokenCleanupBackgroundWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory serviceScopeFactory,
        IOptions<TokenCleanupOptions> options)
        : base( timer, serviceScopeFactory)
    {
        Options = options.Value;
        timer.Period = Options.CleanupPeriod;
    }

    /// <summary>
    /// 执行工作
    /// </summary>
    /// <param name="workerContext"></param>
    /// <returns></returns>
    protected async override Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        await workerContext
            .ServiceProvider
            .GetRequiredService<TokenCleanupService>()
            .CleanAsync();
    }
}
