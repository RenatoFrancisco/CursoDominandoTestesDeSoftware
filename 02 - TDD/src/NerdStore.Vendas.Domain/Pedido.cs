﻿using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public static int MAX_UNIDADE_ITEM = 15;
        public static int MIN_UNIDADE_ITEM = 1;

        public Guid ClienteId { get; private set; }
        public decimal ValorTotal { get; private set; }
        public PedidoStatus PedidoStatus { get; set; }

        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;


        protected Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            if (pedidoItem.Quantidade > MAX_UNIDADE_ITEM) 
                throw new DomainException($"Máximo de {MAX_UNIDADE_ITEM} unidades por produto.");

            if (pedidoItem.Quantidade < MIN_UNIDADE_ITEM)
                throw new DomainException($"Mínimo de {MIN_UNIDADE_ITEM} unidades por produto.");

            if (_pedidoItens.Any(p => p.ProdutoId == pedidoItem.ProdutoId))
            {
                var itemExistente = _pedidoItens.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId);
                itemExistente.AdicionarUnidades(pedidoItem.Quantidade);
                pedidoItem = itemExistente;

                _pedidoItens.Remove(itemExistente);
            }

            _pedidoItens.Add(pedidoItem);
            CalcularValorPedido();
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

        private void CalcularValorPedido() => ValorTotal = _pedidoItens.Sum(pi => pi.CalcularValor());
    }
}
