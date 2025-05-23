﻿using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp;
using Starshine.Abp.Domain.Entities;
using Starshine.Abp.Domain.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionRepositoryExtensions
{
    public static IServiceCollection AddDefaultRepository(
        this IServiceCollection services,
        Type entityType,
        Type repositoryImplementationType,
        bool replaceExisting = false)
    {
        //IReadOnlyBasicRepository<TEntity>
        var readOnlyBasicRepositoryInterface = typeof(IReadOnlyBasicRepository<>).MakeGenericType(entityType);
        if (readOnlyBasicRepositoryInterface.IsAssignableFrom(repositoryImplementationType))
        {
            RegisterService(services, readOnlyBasicRepositoryInterface, repositoryImplementationType, replaceExisting, true);

            //IReadOnlyRepository<TEntity>
            var readOnlyRepositoryInterface = typeof(IReadOnlyRepository<>).MakeGenericType(entityType);
            if (readOnlyRepositoryInterface.IsAssignableFrom(repositoryImplementationType))
            {
                RegisterService(services, readOnlyRepositoryInterface, repositoryImplementationType, replaceExisting, true);
            }

            //IBasicRepository<TEntity>
            var basicRepositoryInterface = typeof(IBasicRepository<>).MakeGenericType(entityType);
            if (basicRepositoryInterface.IsAssignableFrom(repositoryImplementationType))
            {
                RegisterService(services, basicRepositoryInterface, repositoryImplementationType, replaceExisting);

                //IRepository<TEntity>
                var repositoryInterface = typeof(IRepository<>).MakeGenericType(entityType);
                if (repositoryInterface.IsAssignableFrom(repositoryImplementationType))
                {
                    RegisterService(services, repositoryInterface, repositoryImplementationType, replaceExisting);
                }
            }
        }

        var primaryKeyType = EntityHelper.FindPrimaryKeyType(entityType);
        if (primaryKeyType != null)
        {
            //IReadOnlyBasicRepository<TEntity, TKey>
            var readOnlyBasicRepositoryInterfaceWithPk = typeof(IReadOnlyBasicRepository<,>).MakeGenericType(entityType, primaryKeyType);
            if (readOnlyBasicRepositoryInterfaceWithPk.IsAssignableFrom(repositoryImplementationType))
            {
                RegisterService(services, readOnlyBasicRepositoryInterfaceWithPk, repositoryImplementationType, replaceExisting, true);

                //IReadOnlyRepository<TEntity, TKey>
                var readOnlyRepositoryInterfaceWithPk = typeof(IReadOnlyRepository<,>).MakeGenericType(entityType, primaryKeyType);
                if (readOnlyRepositoryInterfaceWithPk.IsAssignableFrom(repositoryImplementationType))
                {
                    RegisterService(services, readOnlyRepositoryInterfaceWithPk, repositoryImplementationType, replaceExisting, true);
                }

                //IBasicRepository<TEntity, TKey>
                var basicRepositoryInterfaceWithPk = typeof(IBasicRepository<,>).MakeGenericType(entityType, primaryKeyType);
                if (basicRepositoryInterfaceWithPk.IsAssignableFrom(repositoryImplementationType))
                {
                    RegisterService(services, basicRepositoryInterfaceWithPk, repositoryImplementationType, replaceExisting);

                    //IRepository<TEntity, TKey>
                    var repositoryInterfaceWithPk = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                    if (repositoryInterfaceWithPk.IsAssignableFrom(repositoryImplementationType))
                    {
                        RegisterService(services, repositoryInterfaceWithPk, repositoryImplementationType, replaceExisting);
                    }
                }
            }
        }

        return services;
    }

    private static void RegisterService(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        bool replaceExisting,
        bool isReadOnlyRepository = false)
    {
        var descriptor = ServiceDescriptor.Transient(serviceType, implementationType);

        if (isReadOnlyRepository)
        {
            services.OnActivated(descriptor, context =>
            {
                var repository = context.Instance.As<IRepository>();
                ObjectHelper.TrySetProperty(repository.As<IRepository>(), x => x.IsChangeTrackingEnabled, _ => false);
            });
        }

        if (replaceExisting)
        {
            services.Replace(descriptor);
        }
        else
        {
            services.TryAdd(descriptor);
        }
    }
}
