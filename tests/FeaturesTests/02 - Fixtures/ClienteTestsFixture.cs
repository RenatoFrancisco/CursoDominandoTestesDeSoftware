using Features.Clientes;
using System;

namespace Features.Tests._02___Fixtures
{
    public class ClienteTestsFixture : IDisposable
    {
        public Cliente GerarClienteValido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "Renato",
                "Francisco",
                DateTime.Now.AddYears(-50),
                "renato@gmail.com",
                true,
                DateTime.Now);

            return cliente;
        }

        public Cliente GerarClienteInvalido()
        {
            var cliente = new Cliente(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "renato@gmail.com",
                true,
                DateTime.Now);

            return cliente;
        }

        public void Dispose()
        {
        }
    }
}
