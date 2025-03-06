using Volo.Abp;

namespace Starshine.Abp.Application.Services;

/// <summary>
/// 所有应用服务都必须实现此接口，以便按照约定进行注册和识别。
/// </summary>
public interface IApplicationService : IRemoteService
{
}