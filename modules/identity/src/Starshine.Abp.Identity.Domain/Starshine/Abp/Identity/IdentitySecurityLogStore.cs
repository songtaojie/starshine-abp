using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.SecurityLog;
using Volo.Abp.Uow;

namespace Starshine.Abp.Identity;

/// <summary>
/// 身份安全日志存储
/// </summary>
[Dependency(ReplaceServices = true)]
public class IdentitySecurityLogStore : ISecurityLogStore, ITransientDependency
{
    /// <summary>
    /// 日志记录
    /// </summary>
    protected ILogger<IdentitySecurityLogStore> Logger { get; }

    /// <summary>
    /// 安全日志配置
    /// </summary>
    protected AbpSecurityLogOptions SecurityLogOptions { get; }
    /// <summary>
    /// 身份安全日志存储库
    /// </summary>
    protected IIdentitySecurityLogRepository IdentitySecurityLogRepository { get; }
    /// <summary>
    /// Guid生成器
    /// </summary>
    protected IGuidGenerator GuidGenerator { get; }
    /// <summary>
    /// 工作单元
    /// </summary>
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="securityLogOptions"></param>
    /// <param name="identitySecurityLogRepository"></param>
    /// <param name="guidGenerator"></param>
    /// <param name="unitOfWorkManager"></param>
    public IdentitySecurityLogStore(
        ILogger<IdentitySecurityLogStore> logger,
        IOptions<AbpSecurityLogOptions> securityLogOptions,
        IIdentitySecurityLogRepository identitySecurityLogRepository,
        IGuidGenerator guidGenerator,
        IUnitOfWorkManager unitOfWorkManager)
    {
        Logger = logger;
        SecurityLogOptions = securityLogOptions.Value;
        IdentitySecurityLogRepository = identitySecurityLogRepository;
        GuidGenerator = guidGenerator;
        UnitOfWorkManager = unitOfWorkManager;
    }
    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="securityLogInfo"></param>
    /// <returns></returns>
    public async Task SaveAsync(SecurityLogInfo securityLogInfo)
    {
        if (!SecurityLogOptions.IsEnabled)
        {
            return;
        }

        using var uow = UnitOfWorkManager.Begin(requiresNew: true);
        await IdentitySecurityLogRepository.InsertAsync(new IdentitySecurityLog(GuidGenerator, securityLogInfo));
        await uow.CompleteAsync();
    }
}
