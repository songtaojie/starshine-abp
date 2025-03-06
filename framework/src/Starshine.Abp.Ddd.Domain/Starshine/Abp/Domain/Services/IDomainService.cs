using Volo.Abp.DependencyInjection;

namespace Starshine.Abp.Domain.Services;

/// <summary>
/// 所有领域服务均可实现此接口，以便按照约定识别它们。
/// </summary>
public interface IDomainService : ITransientDependency
{

}
