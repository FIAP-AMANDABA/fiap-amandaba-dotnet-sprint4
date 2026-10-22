using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Amandaba.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Amandaba.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Amandaba.Tests.Integration
{
    public class AmandabaApiFactory
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Secret"] = TestSecret,
                    ["Jwt:Issuer"] = "amandaba-java",
                    ["Jwt:Audience"] = "amandaba-apis"
                });
            });

            builder.ConfigureServices(services =>
            {
                // Remove a configuracao Oracle anterior para usar somente EF InMemory.
                services.RemoveAll<DbContextOptions<ApplicationContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<ApplicationContext>>();

                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseInMemoryDatabase(
                        "AmandabaIntegrationTests"
                    );
                });

                var serviceProvider =
                    services.BuildServiceProvider();

                using var scope =
                    serviceProvider.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<ApplicationContext>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Seed(context);
            });
        }

        public const string TestSecret = "somente-testes-xunit-chave-que-nao-deve-ser-usada-fora";

        public static string CriarToken(decimal userId, string perfil)
        {
            var claims = new[] { new Claim("sub", $"teste{userId}@exemplo.com"),
                new Claim("userId", userId.ToString()), new Claim("perfil", perfil) };
            var token = new JwtSecurityToken(
                issuer: "amandaba-java", audience: "amandaba-apis", claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestSecret)),
                    SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static void Seed(ApplicationContext context)
        {
            var especie = new EspecieEntity
            {
                IdEspecie = 1,
                Nome = "CACHORRO"
            };

            var pet = new PetEntity
            {
                IdPet = 1,
                IdTutor = 1,
                IdEspecie = 1,
                Nome = "Luna",
                Raca = "Golden Retriever",
                Sexo = "FEMEA",
                DataNascimento = new DateTime(2020, 4, 10),
                Cor = "Dourado",
                Castrado = true,
                DataCadastro = DateTime.Now,
                Status = "ATIVO",
                Especie = especie
            };

            context.Usuarios.Add(new UsuarioEntity { IdUsuario = 1, Nome = "Tutor Teste", Cpf = "11111111111",
                Email = "teste1@exemplo.com", SenhaHash = "teste", DataCadastro = DateTime.UtcNow, Status = "ATIVO" });
            context.Usuarios.Add(new UsuarioEntity { IdUsuario = 2, Nome = "Veterinario Teste", Cpf = "22222222222",
                Email = "teste2@exemplo.com", SenhaHash = "teste", DataCadastro = DateTime.UtcNow, Status = "ATIVO" });
            context.Usuarios.Add(new UsuarioEntity { IdUsuario = 3, Nome = "Outro Tutor", Cpf = "33333333333",
                Email = "teste3@exemplo.com", SenhaHash = "teste", DataCadastro = DateTime.UtcNow, Status = "ATIVO" });
            context.Tutores.Add(new TutorEntity { IdTutor = 1, IdUsuario = 1 });
            context.Tutores.Add(new TutorEntity { IdTutor = 3, IdUsuario = 3 });
            context.Veterinarios.Add(new VeterinarioEntity { IdVeterinario = 7, IdUsuario = 2, Crmv = "SP-123", Status = "ATIVO" });
            context.PetVeterinarios.Add(new PetVeterinarioEntity { IdPetVeterinario = 1,
                IdPet = 1, IdVeterinario = 7, Status = "ATIVO", DataCompartilhamento = DateTime.UtcNow });
            context.Especies.Add(especie);
            context.Pets.Add(pet);

            context.SaveChanges();
        }
    }
}