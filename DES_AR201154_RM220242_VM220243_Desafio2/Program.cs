using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// NOTA: Ocelot no soporta Scalar de forma nativa (ver Guía 7, Parte 4, paso 9).
// Por ello, la documentación interactiva de Scalar se consulta directamente en Eventos.API,
// mientras que el Gateway se prueba con Postman/Insomnia como se explica en la guía.
var ocelotConfigFile = builder.Environment.IsDevelopment() ? "ocelot.json" : "ocelot.Docker.json";
builder.Configuration.AddJsonFile ( ocelotConfigFile, optional: false, reloadOnChange: true );

builder.Services.AddOcelot ( builder.Configuration );

var app = builder.Build();

await app.UseOcelot ();

app.Run ();
