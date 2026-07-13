using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PedidosNet.Data;
using PedidosNet.Models;

namespace PedidosNet.Controllers;

// ⚠️ CRUD de apoio — não é o foco pedagógico do curso, por isso ficou
// deliberadamente simples (sem DTO de resposta, expondo a entidade direto).
[ApiController]
[Route("api/clientes")]
public class ClienteController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClienteController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var clientes = await _context.Clientes.AsNoTracking().ToListAsync();
        return Ok(clientes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(Cliente cliente)
    {
        cliente.Id = Guid.NewGuid();
        cliente.DataCadastro = DateTime.Now;

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return Ok(cliente.Id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, Cliente cliente)
    {
        var existente = await _context.Clientes.FindAsync(id);
        if (existente is null)
            return NotFound();

        existente.Nome = cliente.Nome;
        existente.Email = cliente.Email;
        existente.Vip = cliente.Vip;
        existente.Corporativo = cliente.Corporativo;
        existente.Bloqueado = cliente.Bloqueado;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente is null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
