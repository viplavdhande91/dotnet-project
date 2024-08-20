﻿using Ardalis.SharedKernel;
using Company.FutureProject.Core.ContributorAggregate;
using Company.FutureProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Company.FutureProject.IntegrationTests.Data;

public abstract class BaseEfRepoTestFixture
{
  protected AppDbContext _dbContext;

  protected BaseEfRepoTestFixture()
  {
    var options = CreateNewContextOptions();
    var _fakeEventDispatcher = Substitute.For<IDomainEventDispatcher>();

    _dbContext = new AppDbContext(options, _fakeEventDispatcher);
  }

  protected static DbContextOptions<AppDbContext> CreateNewContextOptions()
  {
    // Create a fresh service provider, and therefore a fresh
    // InMemory database instance.
    var serviceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    // Create a new options instance telling the context to use an
    // InMemory database and the new service provider.
    var builder = new DbContextOptionsBuilder<AppDbContext>();
    builder.UseInMemoryDatabase("cleanarchitecture")
           .UseInternalServiceProvider(serviceProvider);

    return builder.Options;
  }

  protected EfRepository<Contributor> GetRepository()
  {
    return new EfRepository<Contributor>(_dbContext);
  }
}
