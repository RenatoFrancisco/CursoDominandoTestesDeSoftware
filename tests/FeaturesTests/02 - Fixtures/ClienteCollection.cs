using Xunit;

namespace Features.Tests._02___Fixtures
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixture> { }
}
