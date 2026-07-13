namespace PedidosNet.DTOs;

public class CreatePedidoItemRequest
{
    public Guid ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
