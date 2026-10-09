using PedidosNet.Models;

namespace PedidosNet.Domain
{
    public interface IPedidoFactory
    {
        Pedido Criar(Cliente cliente, IReadOnlyCollection<ItemPedido> itens);
    }
}
