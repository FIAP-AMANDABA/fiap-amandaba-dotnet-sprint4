# Amandaba — API de Saúde dos Pets (.NET 8)

API REST para cadastro de pets, acompanhamento de saúde e compartilhamento do histórico clínico entre tutores e veterinários. Desenvolvida para o Challenge 2026 da FIAP, com persistência relacional no Oracle e consulta documental de pesos no MongoDB.

## Funcionalidades

- Gerenciamento de pets vinculados ao tutor.
- Histórico de pesos, vacinas, doenças, alergias, medicamentos, consultas e exames.
- Compartilhamento de pets com veterinários cadastrados e revogação do acesso.
- Autorização por perfil: o tutor gerencia o cadastro e a saúde dos próprios pets; o veterinário autorizado consulta o pet e administra apenas informações de saúde.
- Autenticação JWT Bearer: o token é emitido pela API Java do Amandaba e validado nesta API .NET.
- Listagens de pets do tutor e do veterinário com paginação, filtros/ordenação aplicáveis e links HATEOAS padronizados por `LinkDto`.
- Exportação JSON e consulta do histórico documental de pesos no MongoDB.
- Execução de procedure e função PL/SQL do pacote `PKG_PET_SAUDE`.
- Plano de cuidados por IA com Google Gemini, quando a chave de API estiver configurada.

## Tecnologias

ASP.NET Core 8, C#, Entity Framework Core 8, Oracle Database, Oracle.EntityFrameworkCore, MongoDB.EntityFrameworkCore, MongoDB.Bson, JWT Bearer, Swagger/OpenAPI, Serilog, Health Checks, OpenTelemetry/Azure Monitor, xUnit, Moq e WebApplicationFactory.

## Arquitetura

```text
Front-end
   | 
   +--> API Java (cadastro, login e emissao de JWT)
   |          |
   |          +--> Oracle: TAB_USUARIO, TAB_TUTOR, TAB_VETERINARIO
   |
   +--> API ASP.NET Core (validacao do JWT e autorizacao)
                   |
                   +--> Controllers (HTTP / Swagger)
                            |
                            +--> Application (DTOs / UseCases)
                                      |
                                      +--> Domain (entidades / interfaces)
                                                |
                                                +--> Infrastructure (Repository / EF Core)
                                                          |
                                                          +--> Oracle (pets e saude)
                                                          +--> PKG_PET_SAUDE
                                                          +--> MongoApplicationContext (EF Core)
                                                                    |
                                                                    +--> MongoDB (historico documental de pesos)
```

A organização do código separa `Presentation`, `Application`, `Domain` e `Infrastructure` **em pastas dentro do mesmo projeto** `Amandaba.API.csproj`, como definido para a entrega. Os repositórios abstraem o acesso a dados e são registrados por injeção de dependência em `Program.cs`. A chamada HTTP ao Gemini é implementada em `Infrastructure/Services/GeminiService.cs`, acessada pela interface `IGeminiService` da camada Application. As validações de pet compartilhadas entre casos de uso e testes ficam em `Domain/Rules/PetRules.cs`.

## Instalação e execução

**Pré-requisitos:** .NET SDK 8, acesso ao Oracle com o esquema SQL oficial do Challenge e API Java para emissão dos tokens. O MongoDB é necessário para os endpoints documentais de peso; a integração Gemini depende de uma chave de API.

1. Abra `Amandaba.sln` no Visual Studio ou execute a solução pelo terminal.
2. Copie `Amandaba.API/appsettings.Local.example.json` para `Amandaba.API/appsettings.Local.json`.
3. Preencha a string de conexão `ConnectionStrings:Oracle` no formato `User Id=USUARIO;Password=SENHA;Data Source=SERVIDOR:1521/SERVICO;`.
4. Configure `Jwt:Secret` com a mesma chave de assinatura HS256 utilizada pela API Java. Mantenha `Jwt:Issuer=amandaba-java` e `Jwt:Audience=amandaba-apis`.
5. Para o MongoDB, instale e inicie o MongoDB Community Server no computador (porta 27017). A configuração da aula utiliza `ConnectionStrings:Mongodb` com `mongodb://localhost:27017` e banco `amandaba`. Não é necessária API Key nem conta no Atlas. O arquivo `appsettings.Local.json` está excluído do Git.

```bash
dotnet restore
dotnet build
dotnet run --project Amandaba.API
```

Acesse o Swagger pela URL informada na inicialização, acrescentando `/swagger` (por exemplo, `https://localhost:7019/swagger`). No botão **Authorize**, cole somente o valor do token JWT recebido no login Java.

Para usar variáveis de ambiente em vez do arquivo local: `ConnectionStrings__Oracle`, `Jwt__Secret`, `Jwt__Issuer`, `Jwt__Audience`, `ConnectionStrings__Mongodb` e `Gemini__ApiKey`.

**O esquema Oracle já é administrado pelo script SQL do Challenge.** Não execute scripts de criação nem migrations diretamente sobre o banco compartilhado com o Java.

## Autenticação e controle de acesso

O Java é responsável pelo cadastro e pelo endpoint `POST /api/auth/login`. Seu JWT contém `sub` (e-mail), `userId` (ID de `TAB_USUARIO`) e `perfil` (`TUTOR` ou `VETERINARIO`). O .NET valida assinatura, emissor, audiência e expiração. Em seguida, consulta o Oracle para resolver o ID da tabela de perfil (`TAB_TUTOR` ou `TAB_VETERINARIO`) e as permissões sobre cada pet.

| Perfil | Operações permitidas |
|---|---|
| Tutor proprietário | Cadastrar e atualizar seus pets, gerenciar dados de saúde, compartilhar e revogar vínculos |
| Veterinário com compartilhamento `ATIVO` | Consultar pet e gerenciar registros de saúde |
| Veterinário sem compartilhamento | Acesso ao pet negado |
| Veterinário tentando alterar o cadastro do pet | Acesso negado |

A verificação do compartilhamento é efetuada no Oracle; a revogação passa a valer para novas requisições, mesmo com JWT ainda válido.

## Endpoints REST

A documentação interativa completa, incluindo DTOs de entrada, respostas e parâmetros, está disponível em `/swagger`.

| Recurso | Método e rota |
|---|---|
| Pets do tutor | `GET/POST /api/tutores/{idTutor}/pets` |
| Pet | `GET/PUT /api/pets/{petId}`; `PATCH /api/pets/{petId}/status` |
| Pets do tutor autenticado | `GET /api/pets/me` |
| Pets compartilhados com o veterinário | `GET /api/veterinarios/me/pets` |
| Compartilhamentos | `GET/POST /api/pets/{petId}/veterinarios`; `PATCH /api/pets/{petId}/veterinarios/{veterinarioId}/revogar` |
| Espécies | `GET /api/especies` |
| Catálogo de vacinas | `GET /api/vacinas`; `GET /api/vacinas/{vacinaId}` |
| Peso | `GET/POST /api/pets/{petId}/pesos`; `GET /api/pets/{petId}/pesos/atual`; `PUT/DELETE /api/pets/{petId}/pesos/{pesoId}` |
| Vacinas aplicadas | `GET/POST /api/pets/{petId}/vacinas`; `GET/PUT/DELETE /api/pets/{petId}/vacinas/{aplicacaoId}` |
| Doenças | `GET/POST /api/pets/{petId}/doencas`; `GET/PUT/DELETE /api/pets/{petId}/doencas/{doencaId}` |
| Alergias | `GET/POST /api/pets/{petId}/alergias`; `GET/PUT/DELETE /api/pets/{petId}/alergias/{alergiaId}` |
| Medicamentos | `GET/POST /api/pets/{petId}/medicamentos`; `GET/PUT/DELETE /api/pets/{petId}/medicamentos/{medicamentoId}`; `PATCH /api/pets/{petId}/medicamentos/{medicamentoId}/status` |
| Consultas | `GET/POST /api/pets/{petId}/consultas`; `GET/PUT/DELETE /api/pets/{petId}/consultas/{consultaId}`; `PATCH /api/pets/{petId}/consultas/{consultaId}/status` |
| Exames | `GET/POST /api/pets/{petId}/exames`; `GET/PUT/DELETE /api/pets/{petId}/exames/{exameId}`; `PATCH /api/pets/{petId}/exames/{exameId}/status` |
| Plano de cuidados | `GET /api/pets/{petId}/plano-cuidados` |
| Pesos / MongoDB | `GET /api/pets/{petId}/pesos/exportar-json`; `POST /api/pets/{petId}/pesos/sincronizar`; `GET /api/pets/{petId}/pesos/mongo` |
| Integração PL/SQL | `POST /api/tutores/{idTutor}/integracao-oracle/executar-pr-pet-json`; `GET /api/pets/{petId}/integracao-oracle/idade` |
| Monitoramento | `GET /health/live`; `GET /health/db`; `GET /health/mongo`; `GET /health/external`; `GET /health/ready`; `GET /api/health/live`; `GET /api/health/db`; `GET /api/health/ready` |

As listagens `/api/pets/me` e `/api/veterinarios/me/pets` possuem `pagina` e `tamanho` (limite máximo 100), com consulta `OrderBy`, `Skip` e `Take` no repositório (EF Core). A primeira também oferece `nome` e `ordem` (`nome`/`nome_desc`). O retorno preserva os campos JSON `pagina`, `tamanho`, `total`, `dados` e `links`.

Nas consultas por ID e nas principais listagens clínicas os DTOs retornam `links` em padrão HATEOAS (`href`, `rel` e `method`). Consulta por ID, peso atual e catálogos pequenos não recebem paginação, por não se tratar de listas volumosas. `/api/pets/me` também aceita `nome` e `ordem` (`nome` ou `nome_desc`).

### Respostas e tratamento de erros

| HTTP | Situação |
|---|---|
| `200/201/204` | Consulta, inclusão ou alteração realizada |
| `400` | Requisição inválida ou erro de validação |
| `401` | Token ausente, inválido ou expirado |
| `403` | Usuário sem permissão |
| `404` | Recurso não encontrado |
| `500` | Falha interna inesperada |

As validações usam Data Annotations e regras da camada de aplicação. O tratamento global em `Program.cs` registra erros inesperados via Serilog e retorna a mensagem genérica `{"mensagem":"Erro interno da API."}`, sem expor informações da infraestrutura. Os controladores mantêm respostas específicas para validações e recursos inexistentes.

## Persistência de dados

### Oracle e Entity Framework Core

O Oracle armazena os registros oficiais. O EF Core realiza as consultas e alterações pelo padrão Repository. As migrations da Sprint 3 e Sprint 4 estão em `Amandaba.API/Infrastructure/Data/Migrations/`.

**Decisão para a Sprint 4:** manter os arquivos das migrations no projeto, sem executar `dotnet ef database update` no Oracle compartilhado com o Java. A criação e manutenção das tabelas utilizadas pelos dois backends permanecem sob controle dos scripts SQL do Challenge.

Para verificar a lista de migrations sem alterar o banco, no terminal, com `dotnet-ef` disponível:

```bash
dotnet ef migrations list --project Amandaba.API --startup-project Amandaba.API
```

Atenção: a consulta de migrations pode depender de acesso ao banco; não se declara que essas migrations foram aplicadas. O requisito do professor de **migrations aplicadas** permanece pendente de evidência por decisão do grupo.

### MongoDB: histórico documental de pesos

**Configuração local utilizada na aula:** `ConnectionStrings:Mongodb = mongodb://localhost:27017`, banco `amandaba`, coleção `historico_pesos`. O banco é criado no primeiro registro pelo MongoDB. Consulte também `VALIDACAO_MONGODB_LOCAL.md` para instalar, configurar e testar.

O histórico oficial é armazenado em `TAB_PET_PESO`. O endpoint `POST /api/pets/{petId}/pesos/sincronizar` copia as medições do Oracle para um documento da coleção `historico_pesos` no MongoDB. O endpoint `GET /api/pets/{petId}/pesos/mongo` consulta esse documento. A sincronização é manual. O acesso MongoDB usa `MongoApplicationContext : DbContext`, `DbSet<PesoHistoricoDocumento>`, LINQ e `SaveChanges()`, seguindo o exemplo `PedidoRepository` do professor. A lista de medições permanece incorporada ao documento de cada pet.

A exportação JSON de dados relacionais pode ser realizada pelo endpoint `GET /api/pets/{petId}/pesos/exportar-json` ou pelo script:

```powershell
.\scripts\exportar-pesos-mongodb.ps1 -Token "JWT" -PetIds 1,2,3
mongoimport --uri mongodb://localhost:27017 --db amandaba --collection historico_pesos --file exports/pesos-mongodb.json --jsonArray --mode upsert --upsertFields _id
```

Os IDs devem corresponder a pets existentes e acessíveis ao usuário. O script gera o arquivo `exports/pesos-mongodb.json`, excluído do Git. O documento MongoDB mantém `petId`, `atualizadoEm` e o array `historico` de medições.

### Integração com PL/SQL

O projeto consome o pacote Oracle `PKG_PET_SAUDE` pela camada Infrastructure (Repository): `PR_PET_JSON` (saída por `DBMS_OUTPUT`) e `FN_CALCULAR_IDADE_PET` (idade a partir da data de nascimento). A procedure executa no banco, mas seu `DBMS_OUTPUT` não é capturado pelo endpoint REST. Para visualizar o texto emitido pela procedure, habilite `SERVEROUTPUT` na sessão do SQL Developer.

## Monitoramento e testes

- Health Checks `/health/live` (API), `/health/db` (Oracle), `/health/mongo` (MongoDB), `/health/external` (Gemini) e `/health/ready` (consolidado), além dos endpoints MVC `/api/health/live`, `/api/health/db` e `/api/health/ready`. MongoDB/Gemini sem configuração são marcados como `Degraded`; falhas de conexão retornam `Unhealthy`. A configuração padrão do MongoDB aponta para localhost:27017, de modo que o health check retornará `Unhealthy` até o MongoDB local estar iniciado. A checagem HTTP do Gemini valida a disponibilidade do host, não a validade da chave de API.
- O padrão de Health Check segue a aula 05: `AddHealthChecks`, `IHealthCheck`, tags e `MapHealthChecks`.
- Logging estruturado com Serilog, níveis de log e identificação de rastreamento.
- OpenTelemetry/Azure Monitor quando a connection string estiver configurada.
- Testes unitários e de integração com xUnit, Moq e `WebApplicationFactory`, incluindo regras de JWT, permissões e tratamento HTTP de erros.

```bash
dotnet test
```

Os testes de integração usam EF Core InMemory e não alteram o Oracle de produção. Os testes adicionais verificam HATEOAS, paginação, bloqueio de JWT de usuário inexistente/inativo, estados de Health Check e regras de domínio do pet (status, sexo, nascimento e nome).

Para gerar um relatório de cobertura (Coverlet já consta no projeto de testes):

```bash
dotnet test --collect:"XPlat Code Coverage"
```

**Validação pendente:** gerar e conferir o relatório real de cobertura em um ambiente com SDK .NET 8; a análise estática não comprova a execução dos testes. O GitHub Actions, em `.github/workflows/ci.yml`, executa restauração, compilação e testes em `push` e `pull_request`.

## Equipe

| Integrante | RM | Turma |
|---|---|---|
| Grazielle de Alencar Silva | 561529 | A confirmar |
| Rafael Kubagawa Ramos | 565572 | A confirmar |
| Diego Andrade dos Santos | 566385 | A confirmar |
| Vinicius Soteras Braga | 566230 | A confirmar |
| Julia Correa e Souza de Altino | 564870 | A confirmar |

## Informações pendentes para publicação

- **Turma exata:** confirmar a identificação (por exemplo, 2TDS...) antes de publicar.
- **Repositório GitHub:** inserir o URL real da equipe quando estiver disponível.
- **Ambiente:** não foi confirmada a execução de MongoDB ou Gemini neste ambiente. Os Health Checks criados permitem checar os serviços após configurar as conexões.
- **MongoDB local:** a persistência usa agora o provider `MongoDB.EntityFrameworkCore` 8.3.4 da aula e documento com medições incorporadas; executar os testes manuais antes da entrega.
- **Compilação e testes:** precisam ser executados em computador com .NET SDK 8; não foram validados automaticamente nesta revisão.
