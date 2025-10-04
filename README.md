# FCG – Games API

Microsserviço de **Jogos** da plataforma **FIAP Cloud Games (FCG)**.
Responsável por **catálogo de jogos**, **concessão de jogos (library)**, **sugestões** e **métricas de popularidade** com **Elasticsearch**. Implementado em **.NET 8 (Minimal APIs)** com **EF Core (SQL Server)**, **OpenTelemetry** (tracing) e **Serilog** (logs).

---

## Arquitetura hexagonal (visão geral)

* **Camadas**: Domain / Application / Infrastructure / Api

  * `Fcg.Game.Domain`: entidades (ex.: `GameModel`, `PurchasedGameModel`) e Value Objects.
  * `Fcg.Game.Application`: serviços (`GameService`), portas e DTOs (requests/responses).
  * `Fcg.Game.Infrastructure`: **EF Core** (SQL Server), repositórios, **Elasticsearch** (clientes e serviços).
  * `Fcg.Game.Api`: Minimal API (endpoints), **Health Checks**, **OpenTelemetry**, **Serilog**, **Swagger (dev)**.

* **Busca e Recomendação (Elasticsearch)**
  * Indexa jogos (`elasticgamemodel`) ao criar.
  * Sugestões por **gênero** evitando jogos já comprados.
  * **Popularidade** via agregação por compras (`elasticpurchasedgamemodel`).

* **Observabilidade**
  * OpenTelemetry (ASP.NET/HTTP/EF) + OTLP Exporter.
  * Logs estruturados com Serilog.
  * Health check em `/health` (200/503).

---

## Endpoints

Base local (Dockerfile expõe `5265`): `http://localhost:5265`

* **POST `/games`** – Cadastra um jogo
  **Body (exemplo)**

  ```json
  {
    "title": "Space Academy",
    "description": "Aulas interativas no espaço",
    "genre": 3,
    "releaseDate": "2025-10-01T00:00:00",
    "price": 59.9
  }
  ```

  **201/200** com `gameId` (string).

* **GET `/games`** – Lista jogos disponíveis

* **POST `/games/library`** – Concede jogos ao usuário (usado pela Lambda de pagamento)
  **Body (ex.)**

  ```json
  {
    "purchaseId": "8f4a8b9e-6f6a-4d58-9f9f-8e2f0d0a3b1c",
    "userId": "a1b2c3d4-e5f6-4a7b-8c9d-0e1f2a3b4c5d",
    "purchasedGames": [
      { "gameId": "f0f1f2f3-f4f5-f6f7-f8f9-000102030405" }
    ]
  }
  ```

* **GET `/games/library?userId={guid}`** – Retorna a biblioteca do usuário

* **GET `/games/suggestions?userId={guid}`** – Sugestões com base no histórico/gêneros

* **GET `/games/popular`** – Lista **jogos populares** (agregação no Elasticsearch)
---

## Como rodar (local)

**Pré-requisitos**

* .NET 8 SDK
* SQL Server acessível
* **Elasticsearch** (Cloud ou self-host)

**1) Variáveis (exemplos)**

* `ConnectionStrings__DefaultConnection="Server=localhost;Database=FcgGameDb;User Id=sa;Password=Your!Passw0rd;TrustServerCertificate=True"`
* **Elasticsearch** (use **Address** *ou* **CloudId/Key**):

  * `ElasticSettings__Address="https://<host>:<port>"`
  * `ElasticSettings__CloudId="<cloud-id>"`
  * `ElasticSettings__Key="<api-key>"`

**2) Migrar banco e subir API**

```bash
# na raiz
dotnet restore Fcg.Game.sln
dotnet build   Fcg.Game.sln -c Release

# aplicar migrations (projeto de Infra como -p, API como -s)
dotnet ef database update \
  -p Fcg.Game.Infrastructure \
  -s Fcg.Game.Api

# subir a API
dotnet run --project Fcg.Game.Api
# ou defina a url:
dotnet run --project Fcg.Game.Api --urls http://localhost:5265
```

---

## Como rodar (Docker)

```bash
docker build -t fcg-game-api:dev .
docker run --rm -p 5265:5265 \
  -e ConnectionStrings__DefaultConnection="Server=host.docker.internal;Database=FcgGameDb;User Id=sa;Password=Your!Passw0rd;TrustServerCertificate=True" \
  -e ElasticSettings__CloudId="<cloud-id>" \
  -e ElasticSettings__Key="<api-key>" \
  fcg-game-api:dev
```

---

## Configurações (chaves principais)

| Chave                                  | Exemplo                       | Descrição                       |
| -------------------------------------- | ----------------------------- | ------------------------------- |
| `ConnectionStrings__DefaultConnection` | `Server=...;Database=...;...` | SQL Server (EF Core)            |
| `ElasticSettings__Address`             | `https://...:443`             | Endpoint HTTP do Elasticsearch  |
| `ElasticSettings__CloudId`             | `fcg-elastic:...`             | Cloud ID (Elastic Cloud)        |
| `ElasticSettings__Key`                 | `...`                         | API Key do Elasticsearch        |

---

## Testes
Testes unitários feitos em XUnit e NSubstitute

---

## CI/CD (resumo)

* **Workflow**: `.github/workflows/fcg-game.yml`
  * CI: restore + build + **testes**.
  * Docker build + push para **ECR** (AWS).
  * CD: atualização de **ECS Fargate Service** (cluster/serviço/td definidos no YAML).