using System;

namespace Starshine.Abp.Domain.Repositories;

public interface IEntityChangeTrackingProvider
{
    bool? Enabled { get; }

    IDisposable Change(bool? enabled);
}
