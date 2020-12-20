using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteTestsBogusFixture>
    {
    }
}
