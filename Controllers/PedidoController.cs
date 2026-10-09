using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosNet.Data;
using PedidosNet.DTOs;
using PedidosNet.Models;

namespace PedidosNet.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidoController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Itens)
            .AsNoTracking()
            .ToListAsync();

        return Ok(pedidos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);

        return pedido is null ? NotFound() : Ok(pedido);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CreatePedidoRequest request)
    {
        var cliente = await _context.Clientes.FindAsync(request.ClienteId);
        if (cliente is null)
            return BadRequest("Cliente não encontrado.");

        //Regra 1: cliente bloqueado nem chega a criar pedido
        if (cliente.Bloqueado)
            return BadRequest("Cliente bloqueado não pode criar pedidos.");

        var itens = new List<ItemPedido>();

        foreach (var itemRequest in request.Itens)
        {
            var produto = await _context.Produtos.FindAsync(itemRequest.ProdutoId);
            if (produto is null)
                return BadRequest($"Produto {itemRequest.ProdutoId} não encontrado.");

            itens.Add(new ItemPedido
            {
                Id = Guid.NewGuid(),
                ProdutoId = produto.Id,
                NomeProduto = produto.Nome,
                Preco = produto.Preco,
                Quantidade = itemRequest.Quantidade
            });
        }

        var pedido = new Pedido
        {
            Id = Guid.NewGuid(),
            ClienteId = cliente.Id,
            Status = "Pendente",
            DataCriacao = DateTime.Now,
            Itens = itens
        };

        // Calcula o total baseado nos itens
        pedido.Total = pedido.Itens.Sum(i => i.Preco * i.Quantidade);

        // ⚠️ Regras de negócio direto no controller — o primeiro sinal amarelo
        // piscando no painel . A partir daqui, cada nova regra
        // (cliente corporativo, campanha, valor mínimo...) só engorda este método.
        if (cliente.Vip)
            pedido.Total -= pedido.Total * 0.10m;

        if (pedido.Total > 2000)
            pedido.Total -= pedido.Total * 0.05m;

        //Regra 2: Depois de calcular o total, ainda decidimos o status real
        if (cliente.Vip)
            pedido.Status = "EmAnalise";

        //Regra 3: Cliente corporativo com pedido acima de 5000 precisa de validacao
        if (cliente.Corporativo && pedido.Total > 5000)
            pedido.Status = "AguardandoValidacao";

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return Ok(pedido.Id);
    }
}
