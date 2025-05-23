﻿using System;
using System.Threading.Tasks;
using Acme.BookStore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Acme.BookStore.EntityFrameworkCore;

public class EntityFrameworkCoreBookStoreDbSchemaMigrator
    : IBookStoreDbSchemaMigrator,
        ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreBookStoreDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the BookStoreDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        var dbContext = _serviceProvider.GetRequiredService<BookStoreDbContext>();
        var connectionString = dbContext.Database.GetDbConnection().ConnectionString;
        Console.WriteLine($"Resolved Connection String: {connectionString}");
        await dbContext.Database.MigrateAsync();

    }
}
