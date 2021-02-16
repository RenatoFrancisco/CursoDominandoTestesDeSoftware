using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;
namespace NerdStore.Vendas.Domain.Tests
{
    public class PedidoTests
    {
        private readonly Pedido _pedido;

        public PedidoTests()
        {
            _pedido = Pedido.PedidoFactory.NovoPedidoRascunho(Guid.NewGuid());
        }

        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100m);

            // Act
            _pedido.AdicionarItem(pedidoItem);

            // Assert
            Assert.Equal(200m, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistente_DeveIncrementarUnidadeSomarValores()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedido = Pedido.PedidoFactory.NovoPedidoRascunho(produtoId);
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100m);
            pedido.AdicionarItem(pedidoItem);

            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", 1, 100m);

            // Act
            pedido.AdicionarItem(pedidoItem2);

            // Assert
            Assert.Equal(300, pedido.ValorTotal);
            Assert.Equal(1, pedido.PedidoItens.Count);
            Assert.Equal(3, pedido.PedidoItens.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_UnidadesItemAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => _pedido.AdicionarItem(pedidoItem));

        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemExistenteSomaUnidadesAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 1, 100);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 100);
            _pedido.AdicionarItem(pedidoItem);

            // Act && Assert
            Assert.Throws<DomainException>(() => _pedido.AdicionarItem(pedidoItem2));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemNaoExistenteNaLista_DeveRetornarException()
        {
            // Arrange
            var pedidoAtualizado = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);


            // Act & Assert
            Assert.Throws<DomainException>(() => _pedido.AtualizarItem(pedidoAtualizado));
        }

        [Fact(DisplayName = "Atualizar Item Pedido Valido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItemInvalido_DeveAtualizarQuantidade()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedidoItem = new PedidoItem(produtoId, "Produto Teste", 2, 100);
            _pedido.AdicionarItem(pedidoItem);
            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 100);
            var novaQuantidade = pedidoItemAtualizado.Quantidade;

            // Act
            _pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(novaQuantidade, _pedido.PedidoItens.FirstOrDefault(p => p.ProdutoId == produtoId).Quantidade);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Validar Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_PedidoComProdutosDiferentes_DeveAtualizarValorTotal()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(Guid.NewGuid(), "Produto xpto", 2, 100);
            var pedidoItemExistente2 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            _pedido.AdicionarItem(pedidoItemExistente1);
            _pedido.AdicionarItem(pedidoItemExistente2);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", 5, 15);
            var totalPedido = pedidoItemExistente1.Quantidade * pedidoItemExistente1.ValorUnitario +
                              pedidoItemAtualizado.Quantidade * pedidoItemAtualizado.ValorUnitario;


            // Act
            _pedido.AtualizarItem(pedidoItemAtualizado);

            // Assert
            Assert.Equal(totalPedido, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Atualizar Item Pedido Quantidade Acima do Permitido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AtualizarItemPedido_ItensAcimaDoPermitido_DeveRetornarException()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedidoItemExistente1 = new PedidoItem(produtoId, "Produto Teste", 3, 15);
            _pedido.AdicionarItem(pedidoItemExistente1);

            var pedidoItemAtualizado = new PedidoItem(produtoId, "Produto Teste", Pedido.MAX_UNIDADES_ITEM, 15);

            // Act & Assert
            Assert.Throws<DomainException>(() => _pedido.AtualizarItem(pedidoItemAtualizado));
        }

        [Fact(DisplayName = "Remover Item Pedido Inexistente")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemNaoExisteNaLista_DeveRetornarException()
        {
            // Arrange
            var pedidoItemRemover = new PedidoItem(Guid.NewGuid(), "Produto Teste", 5, 100);

            // Act && Assert
            Assert.Throws<DomainException>(() => _pedido.RemoverItem(pedidoItemRemover));
        }

        [Fact(DisplayName = "Remover Item Pedido Deve Calcular Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void RemoverItemPedido_ItemExistente_DeveAtualizarValorTotal()
        {
            // Arrange
            var produtoId = Guid.NewGuid();
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(produtoId, "Produto xpto", 3, 15);
            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);

            var totalPedido = pedidoItem2.Quantidade * pedidoItem2.ValorUnitario;

            // Act
            _pedido.RemoverItem(pedidoItem1);

            // Assert
            Assert.Equal(totalPedido, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Válido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherValido_DeveRetornarSemErros()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-REAIS", 
                                        null, 
                                        15, 
                                        1, 
                                        DateTime.Now.AddDays(15), 
                                        true, 
                                        false, 
                                        TipoDescontoVoucher.Valor);


            // Act
            var result = _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.True(result.IsValid);

        }

        [Fact(DisplayName = "Aplicar Voucher inválido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void Pedido_AplicarVoucherInvalido_DeveRetornarComErros()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-REAIS", 
                                        null, 
                                        15, 
                                        1, 
                                        DateTime.Now.AddDays(-15), 
                                        true, 
                                        false, 
                                        TipoDescontoVoucher.Valor);


            // Act
            var result = _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Aplicar Voucher Tipo Valor Desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoValorDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "produto xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "produto xpto", 3, 15);

            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-REAIS",
                                      null,
                                      15,
                                      1,
                                      DateTime.Now.AddDays(10),
                                      true,
                                      false,
                                      TipoDescontoVoucher.Valor);

            var valorComDesconto = _pedido.ValorTotal - voucher.ValorDesconto;

            // Act
            _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorComDesconto, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Tipo Percentual Desconto")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_VoucherTipoPercentualDesconto_DeveDescontarDoValorTotal()
        {
            // Arrange
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "produto xpto", 2, 100);
            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "produto xpto", 3, 15);

            _pedido.AdicionarItem(pedidoItem1);
            _pedido.AdicionarItem(pedidoItem2);

            var voucher = new Voucher("PROMO-15-OFF",
                                      15,
                                      null,
                                      1,
                                      DateTime.Now.AddDays(10),
                                      true,
                                      false,
                                      TipoDescontoVoucher.Porcentagem);

            var valorDesconto = (_pedido.ValorTotal * voucher.PercentualDesconto) / 100;
            var valorComDesconto = _pedido.ValorTotal - valorDesconto;

            // Act
            _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(valorComDesconto, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Desconto Excede Valor Total")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_DescontoExcedeValorTotalPedido_PedidoDeveTerValorZero()
        {
            // Arrange
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "produto xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItem);

            var voucher = new Voucher("PROMO-300-REAIS",
                          null,
                          300,
                          1,
                          DateTime.Now.AddDays(10),
                          true,
                          false,
                          TipoDescontoVoucher.Valor);

            // Act
            _pedido.AplicarVoucher(voucher);

            // Assert
            Assert.Equal(0, _pedido.ValorTotal);
        }

        [Fact(DisplayName = "Aplicar Voucher Recalcular Desconto na Modificação do Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AplicarVoucher_ModificarItensPedido_DeveCalcularDescontoValorTotal()
        {
            // Arrange
            var pedidoItem1 = new PedidoItem(Guid.NewGuid(), "Produto Xpto", 2, 100);
            _pedido.AdicionarItem(pedidoItem1);

            var voucher = new Voucher("PROMO-15-OFF", 
                                      null, 
                                      50, 
                                      1,
                                      DateTime.Now.AddDays(10), 
                                      true, 
                                      false, 
                                      TipoDescontoVoucher.Valor);

            _pedido.AplicarVoucher(voucher);

            var pedidoItem2 = new PedidoItem(Guid.NewGuid(), "Produto Teste", 4, 25);

            // Act
            _pedido.AdicionarItem(pedidoItem2);

            // Assert
            var totalEsperado = _pedido.PedidoItens.Sum(i => i.Quantidade * i.ValorUnitario) - voucher.ValorDesconto;
            Assert.Equal(totalEsperado, _pedido.ValorTotal);
        }
    }
}