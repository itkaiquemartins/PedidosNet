# PedidosNet

Uma Web API em **ASP.NET Core** para gestão de clientes, produtos e pedidos, construída como projeto-base de um curso sobre **Design Patterns**.

> ⚠️ **Baseline intencionalmente "ingênuo".** Este projeto representa o *ponto de partida* de uma jornada de refatoração guiada por padrões de projeto. Regras de negócio direto no controller, entidades anêmicas e um `Status` como *magic string* são decisões propositais — cada uma delas é a "dor" que um módulo específico do curso resolve mais adiante (Factory Method, State, Strategy, etc.).

## Objetivos

- Servir como código de apoio para um curso prático de Design Patterns em C#/.NET.
- Demonstrar, de forma realista, os sintomas de um código que cresceu sem cuidado arquitetural: lógica de negócio vazando para a camada HTTP, entidades sem invariantes, ausência de abstrações.
- Evoluir, módulo a módulo, para uma versão com camadas bem definidas e padrões de projeto aplicados sobre o mesmo domínio (pedidos, clientes e produtos).

## Domínio

O sistema modela um cenário simples de e-commerce B2B/B2C:

- **Cliente**: pode ser VIP, corporativo e/ou bloqueado.
- **Produto**: possui categoria, preço e estoque.
- **Pedido**: agrega um cliente e uma lista de itens; aplica descontos (10% para clientes VIP, 5% adicional para pedidos acima de R$ 2.000) no momento da criação.
- **ItemPedido**: nome e preço do produto são copiados no momento da compra (denormalização proposital, para não alterar pedidos já fechados quando o preço do produto mudar).

## Estrutura do projeto

```
PedidosNet/
├── Controllers/          # Endpoints HTTP (Minimal MVC Controllers)
│   ├── ClienteController.cs
│   ├── ProdutoController.cs
│   └── PedidoController.cs
├── Models/                # Entidades de domínio (atualmente anêmicas)
│   ├── Cliente.cs
│   ├── Produto.cs
│   ├── Pedido.cs
│   └── ItemPedido.cs
├── DTOs/                  # Contratos de entrada para criação de pedidos
│   ├── CreatePedidoRequest.cs
│   └── CreatePedidoItemRequest.cs
├── Data/
│   ├── AppDbContext.cs    # DbContext do EF Core (SQLite)
│   └── DbSeeder.cs        # Massa de dados fixa para demonstração em aula
├── Properties/
│   └── launchSettings.json
├── appsettings.json
├── appsettings.Development.json
├── Program.cs             # Composição da aplicação (DI, middlewares, seed)
├── PedidosNet.csproj
└── PedidosNet.http        # Requisições de exemplo para testar a API
```

## Arquitetura

Atualmente o projeto segue uma arquitetura **em camada única** (tudo em um só projeto Web API), típica de baseline de curso:

- **Controllers** conversam diretamente com o `AppDbContext` (sem camada de serviço/repositório).
- **Regras de negócio** (desconto VIP, desconto por valor total) ficam no próprio `PedidoController`.
- **Entidades** (`Models/`) são classes anêmicas, com setters públicos e nenhuma regra própria.
- **Persistência**: EF Core com SQLite, banco criado via `EnsureCreated()` (sem migrations formais) e populado por um seeder fixo em `DbSeeder`.
- **Serialização**: ciclos de referência entre `Pedido` ↔ `ItemPedido` são cortados via `ReferenceHandler.IgnoreCycles`.

Este design é deliberado: a proposta do curso é partir daqui e, módulo a módulo, introduzir padrões de projeto (Factory Method, Strategy, State, Repository, etc.) para resolver os problemas que essa estrutura evidencia.

## Recursos usados

- [.NET 10](https://dotnet.microsoft.com/) / ASP.NET Core Web API (Controllers)
- [Entity Framework Core 10](https://learn.microsoft.com/ef/core/) com provider SQLite
- [Swashbuckle (Swagger/OpenAPI)](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) para documentação interativa
- SQLite como banco de dados local (arquivo `pedidosnet.db`)

## Endpoints principais

| Recurso  | Método | Rota                    | Descrição                              |
|----------|--------|--------------------------|-----------------------------------------|
| Clientes | GET    | `/api/clientes`          | Lista todos os clientes                 |
| Clientes | GET    | `/api/clientes/{id}`     | Obtém um cliente por id                 |
| Clientes | POST   | `/api/clientes`          | Cria um cliente                         |
| Clientes | PUT    | `/api/clientes/{id}`     | Atualiza um cliente                     |
| Clientes | DELETE | `/api/clientes/{id}`     | Remove um cliente                       |
| Produtos | GET    | `/api/produtos`          | Lista todos os produtos                 |
| Produtos | GET    | `/api/produtos/{id}`     | Obtém um produto por id                 |
| Produtos | POST   | `/api/produtos`          | Cria um produto                         |
| Produtos | PUT    | `/api/produtos/{id}`     | Atualiza um produto                     |
| Produtos | DELETE | `/api/produtos/{id}`     | Remove um produto                       |
| Pedidos  | GET    | `/api/pedidos`           | Lista todos os pedidos (com itens)      |
| Pedidos  | GET    | `/api/pedidos/{id}`      | Obtém um pedido por id                  |
| Pedidos  | POST   | `/api/pedidos`           | Cria um pedido (aplica descontos)       |

## Como usar

### Pré-requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download) (versão pinada em `global.json`: `10.0.301`)

### Executando

```bash
# Clone o repositório
git clone https://github.com/macoratti/PedidosNet.git
cd PedidosNet

# Restaure as dependências
dotnet restore

# Execute a aplicação
dotnet run
```

Na primeira execução, o banco SQLite (`pedidosnet.db`) é criado automaticamente e populado com clientes, produtos e pedidos de exemplo.

### Testando a API

Com a aplicação em execução (ambiente `Development`), acesse o Swagger UI para explorar e testar os endpoints:

```
https://localhost:{porta}/swagger
```

A porta exata é definida em `Properties/launchSettings.json`. Alternativamente, use o arquivo `PedidosNet.http` (compatível com a extensão REST Client do VS Code ou o executor HTTP nativo do Visual Studio/Rider) para disparar requisições de exemplo prontas.

## Status do projeto

Este repositório acompanha o andamento do curso e reflete o estágio "ingênuo" inicial do domínio. Módulos futuros devem introduzir, sobre esta mesma base:

- Extração de regras de negócio dos controllers para camadas de serviço/domínio.
- Substituição do `Status` (string) por um modelo mais robusto (ex.: padrão State).
- Criação controlada de entidades (ex.: padrão Factory Method).
- Estratégias de desconto plugáveis (ex.: padrão Strategy).
