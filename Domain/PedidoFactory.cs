using PedidosNet.Models;

namespace PedidosNet.Domain
{
    public class PedidoFactory : IPedidoFactory
    {
        public Pedido Criar(Cliente cliente, IReadOnlyCollection<ItemPedido> itens)
        {
            var pedido = new Pedido
            {
                Id = Guid.NewGuid(),
                ClienteId = cliente.Id,
                DataCriacao = DateTime.Now,
                Itens = itens.ToList()
            };

            pedido.CalcularTotal(); //Quem calcula o total é o Pedido
            pedido.Status = DefinirStatusInicial(cliente, pedido.Total);

            return pedido;
        }

        private static string DefinirStatusInicial(Cliente cliente, decimal total)
        {
            if (cliente.Corporativo && total > 5000)
                return "AguardandoValidacao";

            if (cliente.Vip)
                return "EmAnalise";

            return "Pendente";
        }
    }
}
