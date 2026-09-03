using Eventos.Common;
using Eventos.DAL.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Eventos.BL;
using Eventos.BL.Interfaces;
using AutoMapper;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<AppSettings>(options =>
{
    options.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
});

// Add DAL services (repositories)
builder.Services.AddRepositoryConnector();

// Register Business Layer services and AutoMapper

// Configure AutoMapper and register BL services
builder.Services.AddAutoMapper(typeof(Eventos.BL.Profiles.EventoProfile).Assembly);
builder.Services.AddTransient<IEventoService, EventoService>();
builder.Services.AddTransient<IParticipanteService, ParticipanteService>();
builder.Services.AddTransient<IOrganizadorService, OrganizadorService>();

// Redis distributed cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:Configuration") ?? "localhost:6379";
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Initialize database at startup (wait for completion)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
try
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseInit");
    await Eventos.DAL.DatabaseInitializer.InitializeAsync(connectionString ?? string.Empty, logger);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");
    logger.LogError(ex, "Error initializing database");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();
app.MapControllers();

app.Run();
