﻿namespace Starshine.Abp.Domain.Entities.Events.Distributed;

public interface IEntityToEtoMapper
{
    object? Map(object entityObj);
}
