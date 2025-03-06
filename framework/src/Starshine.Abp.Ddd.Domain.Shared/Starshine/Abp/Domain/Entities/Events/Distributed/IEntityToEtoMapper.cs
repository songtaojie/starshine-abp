using JetBrains.Annotations;

namespace Starshine.Abp.Domain;

/// <summary>
/// 实体到Eto映射器
/// </summary>
public interface IEntityToEtoMapper
{
    /// <summary>
    /// 映射
    /// </summary>
    /// <param name="entityObj"></param>
    /// <returns></returns>
    object? Map(object entityObj);
}
