using PedidosNet.Models;

namespace PedidosNet.Data;

// GUIDs fixos de propósito: em aula, você sempre referencia o "Cliente Ana"
// ou o "Pedido do Bruno" pelo mesmo Id, gravação após gravação.
public static class DbSeeder
{
    public static readonly Guid ClienteAnaId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid ClienteBrunoId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid ClienteCarlaId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid ClienteDaniloId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    public static readonly Guid ProdutoNotebookId = Guid.Parse("aaaaaaaa-0001-0001-0001-000000000001");
    public static readonly Guid ProdutoMouseId = Guid.Parse("aaaaaaaa-0002-0002-0002-000000000002");
    public static readonly Guid ProdutoTecladoId = Guid.Parse("aaaaaaaa-0003-0003-0003-000000000003");
    public static readonly Guid ProdutoMonitorId = Guid.Parse("aaaaaaaa-0004-0004-0004-000000000004");
    public static readonly Guid ProdutoWebcamId = Guid.Parse("aaaaaaaa-0005-0005-0005-000000000005");
    public static readonly Guid ProdutoHeadsetId = Guid.Parse("aaaaaaaa-0006-0006-0006-000000000006");

    public static readonly Guid PedidoAnaId = Guid.Parse("99999999-0001-0001-0001-000000000001");
    public static readonly Guid PedidoBrunoId = Guid.Parse("99999999-0002-0002-0002-000000000002");
    public static readonly Guid PedidoCarlaId = Guid.Parse("99999999-0003-0003-0003-000000000003");

    public static void Seed(AppDbContext db)
    {
        if (db.Clientes.Any())
            return; // banco já populado — evita duplicar a cada restart

        var clientes = new List<Cliente>
        {
            new()
            {
                Id = ClienteAnaId, Nome = "Ana Ferreira", Email = "ana.ferreira@pedidosnet.com.br",
                Vip = true, Corporativo = false, Bloqueado = false,
                DataCadastro = new DateTime(2024, 3, 10)
            },
            new()
            {
                Id = ClienteBrunoId, Nome = "Bruno Salles", Email = "bruno.salles@pedidosnet.com.br",
                Vip = false, Corporativo = true, Bloqueado = false,
                DataCadastro = new DateTime(2024, 6, 22)
            },
            new()
            {
                Id = ClienteCarlaId, Nome = "Carla Nunes", Email = "carla.nunes@pedidosnet.com.br",
                Vip = false, Corporativo = false, Bloqueado = false,
                DataCadastro = new DateTime(2025, 1, 5)
            },
            new()
            {
                Id = ClienteDaniloId, Nome = "Danilo Reis", Email = "danilo.reis@pedidosnet.com.br",
                Vip = false, Corporativo = false, Bloqueado = true,
                DataCadastro = new DateTime(2023, 11, 30)
            },
        };
        db.Clientes.AddRange(clientes);

        var produtos = new List<Produto>
        {
            new() { Id = ProdutoNotebookId, Nome = "Notebook Pro 15\"", Categoria = "Informática", Preco = 6500.00m, Estoque = 12 },
            new() { Id = ProdutoMouseId, Nome = "Mouse Sem Fio", Categoria = "Periféricos", Preco = 90.00m, Estoque = 150 },
            new() { Id = ProdutoTecladoId, Nome = "Teclado Mecânico", Categoria = "Periféricos", Preco = 350.00m, Estoque = 80 },
            new() { Id = ProdutoMonitorId, Nome = "Monitor 27\" 144Hz", Categoria = "Informática", Preco = 1900.00m, Estoque = 25 },
            new() { Id = ProdutoWebcamId, Nome = "Webcam Full HD", Categoria = "Periféricos", Preco = 260.00m, Estoque = 60 },
            new() { Id = ProdutoHeadsetId, Nome = "Headset Gamer", Categoria = "Áudio", Preco = 430.00m, Estoque = 40 },
        };
        db.Produtos.AddRange(produtos);

        var pedidoAna = new Pedido
        {
            Id = PedidoAnaId,
            ClienteId = ClienteAnaId,
            Status = "Pago",
            DataCriacao = new DateTime(2025, 5, 12),
            Itens = new List<ItemPedido>
            {
                new()
                {
                    Id = Guid.Parse("bbbbbbbb-0001-0001-0001-000000000001"),
                    ProdutoId = ProdutoNotebookId, NomeProduto = "Notebook Pro 15\"",
                    Preco = 6500.00m, Quantidade = 1
                },
                new()
                {
                    Id = Guid.Parse("bbbbbbbb-0001-0001-0001-000000000002"),
                    ProdutoId = ProdutoMouseId, NomeProduto = "Mouse Sem Fio",
                    Preco = 90.00m, Quantidade = 1
                },
            }
        };
        pedidoAna.Total = pedidoAna.Itens.Sum(i => i.Preco * i.Quantidade);
        pedidoAna.Total -= pedidoAna.Total * 0.10m; // Ana é VIP: desconto já refletido no histórico

        var pedidoBruno = new Pedido
        {
            Id = PedidoBrunoId,
            ClienteId = ClienteBrunoId,
            Status = "Pendente",
            DataCriacao = new DateTime(2025, 6, 2),
            Itens = new List<ItemPedido>
            {
                new()
                {
                    Id = Guid.Parse("bbbbbbbb-0002-0002-0002-000000000001"),
                    ProdutoId = ProdutoMonitorId, NomeProduto = "Monitor 27\" 144Hz",
                    Preco = 1900.00m, Quantidade = 2
                },
                new()
                {
                    Id = Guid.Parse("bbbbbbbb-0002-0002-0002-000000000002"),
                    ProdutoId = ProdutoTecladoId, NomeProduto = "Teclado Mecânico",
                    Preco = 350.00m, Quantidade = 1
                },
            }
        };
        pedidoBruno.Total = pedidoBruno.Itens.Sum(i => i.Preco * i.Quantidade);

        var pedidoCarla = new Pedido
        {
            Id = PedidoCarlaId,
            ClienteId = ClienteCarlaId,
            Status = "Cancelado",
            DataCriacao = new DateTime(2025, 4, 18),
            Itens = new List<ItemPedido>
            {
                new()
                {
                    Id = Guid.Parse("bbbbbbbb-0003-0003-0003-000000000001"),
                    ProdutoId = ProdutoWebcamId, NomeProduto = "Webcam Full HD",
                    Preco = 260.00m, Quantidade = 1
                },
            }
        };
        pedidoCarla.Total = pedidoCarla.Itens.Sum(i => i.Preco * i.Quantidade);

        db.Pedidos.AddRange(pedidoAna, pedidoBruno, pedidoCarla);

        db.SaveChanges();
    }
}
