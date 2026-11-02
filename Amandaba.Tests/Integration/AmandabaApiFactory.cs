using Amandaba.Domain.Entities;
using Amandaba.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Amandaba.Tests.Integration;

public class AmandabaApiFactory : WebApplicationFactory<Program>
{
    // Chave exclusiva dos testes. A aplicacao normal continua usando a chave do Java.
    public const string TestSecret =
        "somente-testes-xunit-chave-que-nao-deve-ser-usada-fora";

    private readonly InMemoryDatabaseRoot _databaseRoot = new();
    private readonly string _databaseName = $"AmandabaTests_{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, configuracao) =>
        {
            configuracao.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = TestSecret,
                ["Jwt:Issuer"] = "amandaba-java",
                ["Jwt:Audience"] = "amandaba-apis",
                ["ConnectionStrings:Oracle"] = ""
            });
        });

        // ConfigureTestServices aplica as substituicoes depois dos registros da API.
        builder.ConfigureTestServices(services =>
        {
            // EF Core 8 registra tanto DbContextOptions quanto configuracoes internas
            // do contexto. E necessario remover ambas antes de usar InMemory.
            var registrosOracle = services.Where(descriptor =>
                descriptor.ServiceType == typeof(ApplicationContext) ||
                descriptor.ServiceType == typeof(DbContextOptions) ||
                descriptor.ServiceType == typeof(DbContextOptions<ApplicationContext>) ||
                (descriptor.ServiceType.IsGenericType &&
                 descriptor.ServiceType.GetGenericTypeDefinition().Name ==
                     "IDbContextOptionsConfiguration`1" &&
                 descriptor.ServiceType.GenericTypeArguments[0] ==
                     typeof(ApplicationContext))).ToList();

            foreach (var registro in registrosOracle)
                services.Remove(registro);

            services.AddDbContext<ApplicationContext>(options =>
                options.UseInMemoryDatabase(_databaseName, _databaseRoot));

            // Impede que configuracoes locais ou variaveis do computador
            // substituam a assinatura usada nos tokens criados pelos testes.
            services.PostConfigure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(TestSecret)),
                        ValidateIssuer = true,
                        ValidIssuer = "amandaba-java",
                        ValidateAudience = true,
                        ValidAudience = "amandaba-apis",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(1),
                        NameClaimType = "sub",
                        RoleClaimType = "perfil"
                    };
                });

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var contexto = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            contexto.Database.EnsureCreated();
            if (!contexto.Usuarios.Any())
                Seed(contexto);
        });
    }

    public static string CriarToken(decimal userId, string perfil)
    {
        var claims = new[]
        {
            new Claim("sub", $"teste{userId}@exemplo.com"),
            new Claim("userId", userId.ToString()),
            new Claim("perfil", perfil)
        };

        var token = new JwtSecurityToken(
            issuer: "amandaba-java",
            audience: "amandaba-apis",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSecret)),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void Seed(ApplicationContext context)
    {
        var especie = new EspecieEntity { IdEspecie = 1, Nome = "CACHORRO" };
        var tutor = new UsuarioEntity
        {
            IdUsuario = 1, Nome = "Tutor Teste", Cpf = "11111111111",
            Email = "teste1@exemplo.com", SenhaHash = "teste",
            DataCadastro = DateTime.UtcNow, Status = "ATIVO"
        };
        var veterinario = new UsuarioEntity
        {
            IdUsuario = 2, Nome = "Veterinario Teste", Cpf = "22222222222",
            Email = "teste2@exemplo.com", SenhaHash = "teste",
            DataCadastro = DateTime.UtcNow, Status = "ATIVO"
        };
        var outroTutor = new UsuarioEntity
        {
            IdUsuario = 3, Nome = "Outro Tutor", Cpf = "33333333333",
            Email = "teste3@exemplo.com", SenhaHash = "teste",
            DataCadastro = DateTime.UtcNow, Status = "ATIVO"
        };

        var usuarioInativo = new UsuarioEntity
        {
            IdUsuario = 4, Nome = "Usuario Inativo", Cpf = "44444444444",
            Email = "teste4@exemplo.com", SenhaHash = "teste",
            DataCadastro = DateTime.UtcNow, Status = "INATIVO"
        };

        context.Usuarios.AddRange(tutor, veterinario, outroTutor, usuarioInativo);
        context.Tutores.AddRange(
            new TutorEntity { IdTutor = 1, IdUsuario = 1, Usuario = tutor },
            new TutorEntity { IdTutor = 3, IdUsuario = 3, Usuario = outroTutor });
        context.Veterinarios.Add(new VeterinarioEntity
        {
            IdVeterinario = 7, IdUsuario = 2, Crmv = "SP-123", Status = "ATIVO"
        });
        context.Especies.Add(especie);
        context.Pets.Add(new PetEntity
        {
            IdPet = 1, IdTutor = 1, IdEspecie = 1, Nome = "Luna",
            Raca = "Golden Retriever", Sexo = "FEMEA",
            DataNascimento = new DateTime(2020, 4, 10),
            Cor = "Dourado", Castrado = true,
            DataCadastro = DateTime.UtcNow, Status = "ATIVO",
            Especie = especie
        });
        context.PetVeterinarios.Add(new PetVeterinarioEntity
        {
            IdPetVeterinario = 1, IdPet = 1, IdVeterinario = 7,
            Status = "ATIVO", DataCompartilhamento = DateTime.UtcNow
        });
        context.SaveChanges();
    }
}
