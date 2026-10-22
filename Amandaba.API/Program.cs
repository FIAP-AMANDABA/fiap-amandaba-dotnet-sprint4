using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Amandaba.API.Application.Interfaces;
using Amandaba.API.Domain.Interfaces;
using Amandaba.API.Application.UseCases;
using Amandaba.API.Presentation.Filters;
using Amandaba.API.Infrastructure.Mongo;
using Amandaba.Application.Interfaces;
using Amandaba.Application.UseCases;
using Amandaba.Domain.Interfaces;
using Amandaba.Infrastructure.Data;
using Amandaba.Infrastructure.Data.Repositories;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry;
using Oracle.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);


// Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override(
        "Microsoft.AspNetCore",
        LogEventLevel.Warning
    )
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " +
            "[{Level:u3}] " +
            "[TraceId:{TraceId}] " +
            "[SpanId:{SpanId}] " +
            "{SourceContext}{NewLine}" +
            "    {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        path: Path.Combine(
            AppContext.BaseDirectory,
            "logs",
            "api-.log"
        ),
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate:
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} " +
            "[{Level:u3}] " +
            "[TraceId:{TraceId}] " +
            "[SpanId:{SpanId}] " +
            "{SourceContext}{NewLine}" +
            "    {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Services.AddSerilog();

// Controllers
builder.Services.AddScoped<IPetAccessService, PetAccessService>();
builder.Services.AddScoped<PetAccessFilter>();
builder.Services.AddControllers(options => options.Filters.AddService<PetAccessFilter>());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context => new BadRequestObjectResult(new
    {
        mensagem = "Dados inválidos.",
        erros = context.ModelState
            .Where(item => item.Value?.Errors.Count > 0)
            .ToDictionary(item => item.Key,
                item => item.Value!.Errors.Select(error => error.ErrorMessage).ToArray())
    });
});

var chaveJwt = builder.Configuration["Jwt:Secret"];
if (string.IsNullOrWhiteSpace(chaveJwt))
{
    if (!builder.Environment.IsEnvironment("Testing"))
        throw new InvalidOperationException("Configure Jwt:Secret em appsettings.Local.json ou Jwt__Secret.");
    chaveJwt = "somente-testes-xunit-chave-que-nao-deve-ser-usada-fora";
}
if (Encoding.UTF8.GetByteCount(chaveJwt) < 32)
    throw new InvalidOperationException("Jwt:Secret deve possuir pelo menos 32 bytes UTF-8, como no Java.");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chaveJwt)),
            ValidateIssuer = true, ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true, ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true, ClockSkew = TimeSpan.FromMinutes(1),
            NameClaimType = "sub", RoleClaimType = "perfil"
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));


// Entity Framework + Oracle
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseOracle(
        builder.Configuration.GetConnectionString("Oracle"),
        oracleOptions =>
        {
            oracleOptions.UseOracleSQLCompatibility(
                OracleSQLCompatibility.DatabaseVersion21
            );
        }
    );
});

// Repositories
builder.Services.AddTransient<IPetRepository, PetRepository>();
builder.Services.AddTransient<IEspecieRepository, EspecieRepository>();
builder.Services.AddTransient<IPesoRepository, PesoRepository>();
builder.Services.AddTransient<IVacinaRepository, VacinaRepository>();
builder.Services.AddTransient<IDoencaRepository, DoencaRepository>();
builder.Services.AddTransient<IAlergiaRepository, AlergiaRepository>();
builder.Services.AddTransient<IMedicamentoRepository, MedicamentoRepository>();
builder.Services.AddTransient<IConsultaRepository, ConsultaRepository>();
builder.Services.AddTransient<IExameRepository, ExameRepository>();
builder.Services.AddScoped<ICompartilhamentoRepository, CompartilhamentoRepository>();
builder.Services.AddScoped<IPesoMongoRepository, PesoMongoRepository>();
builder.Services.AddScoped<ICompartilhamentoUseCase, CompartilhamentoUseCase>();

// UseCases
builder.Services.AddTransient<IPetUseCase, PetUseCase>();
builder.Services.AddTransient<IEspecieUseCase, EspecieUseCase>();
builder.Services.AddTransient<IPesoUseCase, PesoUseCase>();
builder.Services.AddTransient<IVacinaUseCase, VacinaUseCase>();
builder.Services.AddTransient<IDoencaUseCase, DoencaUseCase>();
builder.Services.AddTransient<IAlergiaUseCase, AlergiaUseCase>();
builder.Services.AddTransient<IMedicamentoUseCase, MedicamentoUseCase>();
builder.Services.AddTransient<IConsultaUseCase, ConsultaUseCase>();
builder.Services.AddTransient<IExameUseCase, ExameUseCase>();

// Health Checks - Oracle e registrado apenas se a conexao estiver configurada.
var healthChecks = builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: ["live"]);
var conexaoOracle = builder.Configuration.GetConnectionString("Oracle");
if (!string.IsNullOrWhiteSpace(conexaoOracle))
{
    healthChecks.AddOracle(connectionString: conexaoOracle, name: "oracle",
        failureStatus: HealthStatus.Unhealthy, tags: ["db"]);
}
else
{
    healthChecks.AddCheck("oracle", () => HealthCheckResult.Unhealthy("Conexao Oracle nao configurada."),
        tags: ["db"]);
}

// Application Insights
if (!string.IsNullOrWhiteSpace(builder.Configuration["ApplicationInsights:ConnectionString"]))
{
    builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
        options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"]);
}

// Gemini
builder.Services.AddHttpClient<GeminiUseCase>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization", Type = SecuritySchemeType.Http,
        Scheme = "bearer", BearerFormat = "JWT", In = ParameterLocation.Header,
        Description = "Cole o JWT fornecido pelo login Java."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme { Reference = new OpenApiReference {
            Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
    });
});

var app = builder.Build();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var erro = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    var codigo = erro switch
    {
        ArgumentException => StatusCodes.Status400BadRequest,
        KeyNotFoundException => StatusCodes.Status404NotFound,
        UnauthorizedAccessException => StatusCodes.Status403Forbidden,
        _ => StatusCodes.Status500InternalServerError
    };

    if (codigo == StatusCodes.Status500InternalServerError)
        app.Logger.LogError(erro, "Erro inesperado na API");
    else
        app.Logger.LogWarning("Requisicao rejeitada: {TipoErro}", erro?.GetType().Name);

    var mensagem = erro switch
    {
        ArgumentException argumento => argumento.Message,
        KeyNotFoundException naoEncontrado => naoEncontrado.Message,
        UnauthorizedAccessException => "Sem permissão para esta operação.",
        _ => "Erro interno da API."
    };

    context.Response.StatusCode = codigo;
    await context.Response.WriteAsJsonAsync(new { mensagem });
}));


app.UseCors("AllowAll");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health Check - Liveness
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live")
});

// Health Check - Readiness / Oracle
app.MapHealthChecks("/health/db", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("db")
});

app.Run();

public partial class Program { }