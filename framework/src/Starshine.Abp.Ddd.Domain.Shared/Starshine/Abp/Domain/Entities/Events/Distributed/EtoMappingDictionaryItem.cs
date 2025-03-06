using System;

namespace Starshine.Abp.Domain;

/// <summary>
/// Etoӳ���ֵ���
/// </summary>
/// <remarks>
/// ���캯��
/// </remarks>
/// <param name="etoType"></param>
/// <param name="objectMappingContextType"></param>
public class EtoMappingDictionaryItem(Type etoType, Type? objectMappingContextType = null)
{
    /// <summary>
    /// Eto����
    /// </summary>
    public Type EtoType { get; } = etoType;

    /// <summary>
    /// ����ӳ������������
    /// </summary>
    public Type? ObjectMappingContextType { get; } = objectMappingContextType;
}
