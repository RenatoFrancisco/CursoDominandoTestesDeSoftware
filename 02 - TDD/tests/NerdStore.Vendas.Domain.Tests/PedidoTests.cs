using System;
using Xunit;
namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {

        [Fact(DisplayName = "Adcionar Item Novo Pedido")]
        [Trait("Categoria", "Pedido Tests")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedido = new Pedido();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100m);

            // Act
            pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200m, pedido.ValorTotal);
        }
    }
}