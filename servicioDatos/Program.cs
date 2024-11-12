using Microsoft.EntityFrameworkCore;
using servicioDatos.Model;
using servicioDatos.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Logging;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar el logging para consola
        builder.Logging.ClearProviders();  // Elimina los proveedores de log predeterminados
        builder.Logging.AddConsole();  // Agrega el proveedor de log para consola

        builder.Services.AddDbContext<EspacioDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("espacios");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'espacios' is null or empty.");
            }
            options.UseMySQL(connectionString);
        });

        // Agregar servicios de gRPC
        builder.Services.AddGrpc();
        builder.Services.AddScoped<EspacioServiceImpl>();

        var app = builder.Build();

        // Mapeo de servicio gRPC
        app.MapGrpcService<EspacioServiceImpl>();

        app.Run();
    }
}
