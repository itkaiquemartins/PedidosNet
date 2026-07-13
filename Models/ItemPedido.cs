namespace PedidosNet.Models;

public class ItemPedido
{
    public Guid Id { get; set; }

    public Guid PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public Guid ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    // Nome e preço copiados do Produto no momento
    // da compra (denormalização
    // proposital: sem isso, mudar o preço de um
    // Produto alteraria pedidos
    // já fechados).
    // É correto — mas ainda não está formalizado como decisão
    // de design; no baseline ingênuo é só "assim que funcionou".
    public string NomeProduto { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
}
