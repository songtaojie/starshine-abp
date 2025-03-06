namespace Starshine.Abp.Application.Services;

/// <summary>
/// 创建更新应用服务
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface ICreateUpdateAppService<TEntityDto, in TKey>
    : ICreateUpdateAppService<TEntityDto, TKey, TEntityDto, TEntityDto>
{

}

/// <summary>
/// 创建更新应用服务
/// </summary>
/// <typeparam name="TEntityDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TCreateUpdateInput"></typeparam>
public interface ICreateUpdateAppService<TEntityDto, in TKey, in TCreateUpdateInput>
    : ICreateUpdateAppService<TEntityDto, TKey, TCreateUpdateInput, TCreateUpdateInput>
{

}

/// <summary>
/// 创建更新应用服务
/// </summary>
/// <typeparam name="TGetOutputDto"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TCreateUpdateInput"></typeparam>
/// <typeparam name="TUpdateInput"></typeparam>
public interface ICreateUpdateAppService<TGetOutputDto, in TKey, in TCreateUpdateInput, in TUpdateInput>
    : ICreateAppService<TGetOutputDto, TCreateUpdateInput>,
        IUpdateAppService<TGetOutputDto, TKey, TUpdateInput>
{

}
