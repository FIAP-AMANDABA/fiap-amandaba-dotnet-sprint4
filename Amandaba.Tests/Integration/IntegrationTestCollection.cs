using Xunit;

namespace Amandaba.Tests.Integration
{
    [CollectionDefinition("Integration")]
    public class IntegrationTestCollection
        : ICollectionFixture<AmandabaApiFactory>
    {
    }
}