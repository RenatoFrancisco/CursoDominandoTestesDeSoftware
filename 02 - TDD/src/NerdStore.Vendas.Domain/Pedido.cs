using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public decimal ValorTotal { get; private set; }

        private readonly List<PedidoItem> _pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => _pedidoItens;

        public Pedido()
        {
            _pedidoItens = new List<PedidoItem>();
        }

        public void AdicionarItem(PedidoItem pedidoItem)
        {
            _pedidoItens.Add(pedidoItem);
            ValorTotal = _pedidoItens.Sum(pi => pi.Quantidade * pi.ValorUnitario);
        }
    }
}
