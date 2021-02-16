using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Pedido : Entity, IAggregateRoot
    {
        public static int MAX_UNIDADES_ITEM = 15;
        public static int MIN_UNIDADES_ITEM = 1;

        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; set; }
        public bool VoucherUtilizado { get; private set; }
        public Voucher Voucher { get; private set; }
        public decimal Desconto { get; private set; }

        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;

        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            ValidarQuantidadeItemPermitida(pedidoItem);

            if (PedidoItemExistente(pedidoItem))
            {
                var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;

                _pedidoItens.Remove(itemExistente);
            }

            _pedidoItens.Add(pedidoItem);
            CalcularValorPedido();
        }

        public void AtualizarItem(PedidoItem pedidoItem)
        {
            ValidarPedidoItemInexistente(pedidoItem);
            ValidarQuantidadeItemPermitida(pedidoItem);

            var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);

            _pedidoItens.Remove(itemExistente);
            _pedidoItens.Add(pedidoItem);

            CalcularValorPedido();
        }

        public void RemoverItem(PedidoItem item)
        {
            ValidarPedidoItemInexistente(item);

            _pedidoItens.Remove(item);
            CalcularValorPedido();
        }

        public bool PedidoItemExistente(PedidoItem item) => _pedidoItens.Any(p => p.ProdutoId == item.ProdutoId);

        public ValidationResult AplicarVoucher(Voucher voucher)
        {
            var result = voucher.ValidarSeAplicavel();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorTotalDesconto();

            return result;
        }

        public void TornarRascunho() => PedidoStatus = PedidoStatus.Rascunho;

        public static class PedidoFactory
        {
            public static Pedido NovoPedidoRascunho(Guid clienteId)
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                };

                pedido.TornarRascunho();
                return pedido;
            }
        }

        private void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            var desconto = 0m;
            var valor = ValorTotal;

            if (Voucher.TipoDescontoVoucher == TipoDescontoVoucher.Valor)
                desconto = Voucher?.ValorDesconto ?? 0m;
            else
                if (Voucher.PercentualDesconto.HasValue)
                    desconto = (ValorTotal * Voucher.PercentualDesconto.Value) / 100;

            valor -= desconto;
            ValorTotal = valor < 0 ? 0 : valor; 
            Desconto = desconto;
        }

        private void ValidarQuantidadeItemPermitida(PedidoItem item)
        {
            var quantidadeItens = item.Quantidade;
            if (PedidoItemExistente(item))
            {
                var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == item.ProdutoId);
                quantidadeItens += itemExistente.Quantidade;
            }

            if (quantidadeItens > MAX_UNIDADES_ITEM)
                throw new DomainException($"Máximo de {MAX_UNIDADES_ITEM} unidades por produto.");
        }


        private void ValidarPedidoItemInexistente(PedidoItem item)
        {
            if (!PedidoItemExistente(item))
                throw new DomainException("O item não pertence ao pedido.");
        }

        private void CalcularValorPedido() 
        {
            ValorTotal = _pedidoItens.Sum(pi => pi.CalcularValor());
            CalcularValorTotalDesconto();
        } 
    }
}
