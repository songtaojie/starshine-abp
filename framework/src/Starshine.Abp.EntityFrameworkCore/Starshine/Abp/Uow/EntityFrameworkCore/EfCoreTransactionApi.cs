using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Starshine.Abp.EntityFrameworkCore;
using Volo.Abp.Threading;
using Volo.Abp.Uow;

namespace Starshine.Abp.Uow.EntityFrameworkCore;

public class EfCoreTransactionApi : ITransactionApi, ISupportsRollback
{
    public IDbContextTransaction DbContextTransaction { get; }
    public IEfCoreDbContext StarterDbContext { get; }
    public List<IEfCoreDbContext> AttendedDbContexts { get; }

    protected ICancellationTokenProvider CancellationTokenProvider { get; }

    public EfCoreTransactionApi(
        IDbContextTransaction dbContextTransaction,
        IEfCoreDbContext starterDbContext,
        ICancellationTokenProvider cancellationTokenProvider)
    {
        DbContextTransaction = dbContextTransaction;
        StarterDbContext = starterDbContext;
        CancellationTokenProvider = cancellationTokenProvider;
        AttendedDbContexts = new List<IEfCoreDbContext>();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.As<DbContext>().HasRelationalTransactionManager() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue; //Relational databases use the shared transaction if they are using the same connection
            }

            await dbContext.Database.CommitTransactionAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        await DbContextTransaction.CommitAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
    }

    public void Dispose()
    {
        DbContextTransaction.Dispose();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        foreach (var dbContext in AttendedDbContexts)
        {
            if (dbContext.As<DbContext>().HasRelationalTransactionManager() &&
                dbContext.Database.GetDbConnection() == DbContextTransaction.GetDbTransaction().Connection)
            {
                continue; //Relational databases use the shared transaction if they are using the same connection
            }

            await dbContext.Database.RollbackTransactionAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
        }

        await DbContextTransaction.RollbackAsync(CancellationTokenProvider.FallbackToProvider(cancellationToken));
    }
}
