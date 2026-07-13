using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosNet.Data;
using PedidosNet.Models;

namespace PedidosNet.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutoController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProdutoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var produtos = await _context.Produtos.AsNoTracking().ToListAsync();
        return Ok(produtos);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        return produto is null ? NotFound() : Ok(produto);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Produto produto)
    {
        produto.Id = Guid.NewGuid();

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return Ok(produto.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, Produto produto)
    {
        var existente = await _context.Produtos.FindAsync(id);
        if (existente is null)
            return NotFound();

        existente.Nome = produto.Nome;
        existente.Categoria = produto.Categoria;
        existente.Preco = produto.Preco;
        existente.Estoque = produto.Estoque;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto is null)
            return NotFound();

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
