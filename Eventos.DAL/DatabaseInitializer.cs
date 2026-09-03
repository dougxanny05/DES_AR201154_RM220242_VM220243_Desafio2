using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;
using System.IO;
using System.Linq;

namespace Eventos.DAL
{
    /// <summary>
    /// Crea la base de datos y las tablas (si no existen) usando el script SQL
    /// embebido en Eventos.DAL/Scripts/CrearBaseDatos.sql.
    ///
    /// Se ejecuta una vez al iniciar Eventos.API (tanto en local como en Docker),
    /// de forma equivalente a un "db.Database.Migrate()" de Entity Framework,
    /// pero usando SQL puro porque esta capa trabaja con Dapper (Guía 4).
    ///
    /// Es IDEMPOTENTE: si la base de datos y las tablas ya existen, no hace nada
    /// destructivo, por lo que es seguro llamarlo en cada arranque del contenedor.
    /// Incluye reintentos porque, en Docker, el contenedor de SQL Server puede
    /// tardar unos segundos más en aceptar conexiones aunque su healthcheck ya
    /// haya pasado.
    /// </summary>
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(string connectionString, ILogger logger, int maxRetries = 10, int delaySeconds = 3)
        {
            var batches = SplitIntoBatches(LoadEmbeddedScript());

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    await EnsureDatabaseExistsAsync(connectionString);
                    await ExecuteBatchesAsync(connectionString, batches);
                    logger.LogInformation("Base de datos 'GestionEventos' verificada/creada correctamente.");
                    return;
                }
                catch (Exception ex)
                {
                    if (attempt == maxRetries)
                    {
                        logger.LogError(ex, "No fue posible inicializar la base de datos tras {MaxRetries} intentos. " +
                            "La API continuará iniciando; verifique la cadena de conexión y que SQL Server esté disponible.", maxRetries);
                        return;
                    }

                    logger.LogWarning("Intento {Attempt}/{MaxRetries} fallido al inicializar la base de datos ({Message}). Reintentando en {Delay}s...",
                        attempt, maxRetries, ex.Message, delaySeconds);
                    await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
                }
            }
        }

        private static async Task EnsureDatabaseExistsAsync(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var targetDatabase = builder.InitialCatalog;

            // Para poder crear la base de datos primero hay que conectarse a "master",
            // ya que la base de datos objetivo todavía podría no existir.
            builder.InitialCatalog = "master";

            using var connection = new SqlConnection(builder.ConnectionString);
            await connection.OpenAsync();

            var sql = $"""
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{targetDatabase}')
                BEGIN
                    CREATE DATABASE [{targetDatabase}];
                END
                """;

            using var command = new SqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }

        private static async Task ExecuteBatchesAsync(string connectionString, IEnumerable<string> batches)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            foreach (var batch in batches)
            {
                if (string.IsNullOrWhiteSpace(batch))
                {
                    continue;
                }

                using var command = new SqlCommand(batch, connection);
                await command.ExecuteNonQueryAsync();
            }
        }

        private static string LoadEmbeddedScript()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "Eventos.DAL.Scripts.CrearBaseDatos.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }

            // Fallback: buscar el archivo SQL en disco (varios niveles por si el working dir cambia)
            var possiblePaths = new[] {
                Path.Combine(AppContext.BaseDirectory, "CrearBaseDatos.sql"),
                Path.Combine(AppContext.BaseDirectory, "..", "CrearBaseDatos.sql"),
                Path.Combine(AppContext.BaseDirectory, "..", "..", "CrearBaseDatos.sql"),
                Path.Combine(Directory.GetCurrentDirectory(), "CrearBaseDatos.sql")
            };

            var found = possiblePaths.FirstOrDefault(File.Exists);
            if (found != null)
            {
                return File.ReadAllText(found);
            }

            // Último recurso: listar recursos embebidos para ayudar al diagnóstico
            var names = string.Join(", ", assembly.GetManifestResourceNames());
            throw new InvalidOperationException($"No se encontró el recurso embebido '{resourceName}' ni el archivo CrearBaseDatos.sql en disco. Recursos disponibles: {names}");
        }

        /// <summary>
        /// Separa el script en lotes usando "GO" como delimitador (igual que hace
        /// SSMS), y descarta la sentencia "USE GestionEventos" porque la conexión
        /// ya apunta directamente a esa base de datos.
        /// </summary>
        private static List<string> SplitIntoBatches(string script)
        {
            var lines = script.Replace("\r\n", "\n").Split('\n');
            var batches = new List<string>();
            var current = new StringBuilder();

            foreach (var line in lines)
            {
                if (line.Trim().Equals("GO", StringComparison.OrdinalIgnoreCase))
                {
                    if (current.Length > 0)
                    {
                        batches.Add(current.ToString());
                        current.Clear();
                    }
                }
                else
                {
                    current.AppendLine(line);
                }
            }

            if (current.Length > 0)
            {
                batches.Add(current.ToString());
            }

            // Filtra los lotes que no empiezan con "USE " y devuelve una lista
            return batches.Where(b => !b.TrimStart().StartsWith("USE ", StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
