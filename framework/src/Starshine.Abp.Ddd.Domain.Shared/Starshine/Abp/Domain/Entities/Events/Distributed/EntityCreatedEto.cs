using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Starshine.Abp.Domain.Entities.Events.Distributed;

/// <summary>
/// 实体创建事件传输对象
/// </summary>
/// <typeparam name="TEntityEto"></typeparam>
[Serializable]
[GenericEventName(Postfix = ".Created")]
public class EntityCreatedEto<TEntityEto> : IEventDataMayHaveTenantId
{
    /// <summary>
    /// 实体事件传输对象
    /// </summary>
    public TEntityEto Entity { get; set; }

    /// <summary>
    /// 实体创建事件传输对象
    /// </summary>
    /// <param name="entity"></param>
    public EntityCreatedEto(TEntityEto entity)
    {
        Entity = entity;
    }

    /// <summary>
    /// 是否是多租户
    /// </summary>
    /// <param name="tenantId"></param>
    /// <returns></returns>
    public virtual bool IsMultiTenant(out Guid? tenantId)
    {
        if (Entity is IMultiTenant multiTenantEntity)
        {
            tenantId = multiTenantEntity.TenantId;
            return true;
        }

        tenantId = null;
        return false;
    }
}
