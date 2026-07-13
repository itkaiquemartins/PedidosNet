namespace PedidosNet.Models;

// ⚠️ Entidade anêmica: é o ponto de partida "ingênuo" do curso.
// Setters públicos, nenhuma regra de negócio,
// nenhuma invariante protegida.
// Isso muda a partir do Módulo 1 (Factory Method).
public class Cliente
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Vip { get; set; }
    public bool Corporativo { get; set; }
    public bool Bloqueado { get; set; }
    public DateTime DataCadastro { get; set; }
}
