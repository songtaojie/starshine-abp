using System;

namespace Starshine.Abp.Domain;

/// <summary>
/// Eto映射字典项
/// </summary>
/// <remarks>
/// 构造函数
/// </remarks>
/// <param name="etoType"></param>
/// <param name="objectMappingContextType"></param>
public class EtoMappingDictionaryItem(Type etoType, Type? objectMappingContextType = null)
{
    /// <summary>
    /// Eto类型
    /// </summary>
    public Type EtoType { get; } = etoType;

    /// <summary>
    /// 对象映射上下文类型
    /// </summary>
    public Type? ObjectMappingContextType { get; } = objectMappingContextType;
}
