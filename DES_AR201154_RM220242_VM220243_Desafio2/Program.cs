using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// NOTA: Ocelot no soporta Scalar de forma nativa (ver Guía 7, Parte 4, paso 9).
// Por ello, la documentación interactiva de Scalar se consulta directamente en Eventos.API,
// mientras que el Gateway se prueba con Postman/Insomnia como se explica en la guía.
var ocelotConfigFileName = builder.Environment.IsDevelopment() ? "ocelot.json" : "ocelot.Docker.json";

// Try several locations so the gateway can run both from Docker and from Visual Studio.
string[] candidates = new[] {
    Path.Combine(builder.Environment.ContentRootPath, ocelotConfigFileName), // project folder / output folder
    Path.Combine(Directory.GetCurrentDirectory(), ocelotConfigFileName), // current working dir
    Path.Combine(AppContext.BaseDirectory, ocelotConfigFileName), // published app folder (Docker /app)
    Path.Combine(builder.Environment.ContentRootPath, "..", ocelotConfigFileName) // repo root
};

string? found = candidates.FirstOrDefault(File.Exists);
if (found == null)
{
    // Provide clearer error message for developers instead of FileNotFoundException
    var tried = string.Join("; ", candidates);
    throw new FileNotFoundException($"No se encontró el archivo de configuración de Ocelot ('{ocelotConfigFileName}'). Se buscaron: {tried}");
}

builder.Configuration.AddJsonFile(found, optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.Run();
