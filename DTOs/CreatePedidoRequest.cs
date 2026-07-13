namespace PedidosNet.DTOs;

public class CreatePedidoRequest
{
    public Guid ClienteId { get; set; }
    public List<CreatePedidoItemRequest> Itens { get; set; } = new();
}
