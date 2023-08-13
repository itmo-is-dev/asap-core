using Xunit;

namespace Itmo.Dev.Asap.Core.Tests.Fixtures;

[CollectionDefinition(nameof(CoreDatabaseCollectionFixture))]
public class CoreDatabaseCollectionFixture : ICollectionFixture<CoreDatabaseFixture> { }