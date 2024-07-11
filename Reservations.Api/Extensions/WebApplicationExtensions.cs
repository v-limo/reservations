namespace Reservations.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task ApplyDatabaseMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();

            var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

            logger.LogInformation($"Applied Migrations: {string.Join(", ", appliedMigrations)}");

            var migrations = pendingMigrations as string[] ?? pendingMigrations.ToArray();
            if (migrations.Length != 0)
            {
                logger.LogInformation($"Pending Migrations: {string.Join(", ", migrations)}");
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully!");
            }
            else
            {
                logger.LogInformation("No pending migrations to apply.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during database migration.");
            // throw;  // TODO: this create an endless loop when combined with global error handling
        }
    }

    public static void AddTestEndpoint(this WebApplication app)
    {
        app.MapGet("/", context =>
        {
            var rootApiMessage = new
            {
                Message = "Yeey! The app is up and running smoothly!",
                PossibleEndpoints = (string[])
                [
                    "http://localhost:5099/swagger/index.html", "http://localhost:5099/api/v1/books",
                    "http://<deployed-link>/api/v1/books"
                ],
                MoreInfo = "Read the app docs of docker image info for more infomation"
            };
            return context.Response.WriteAsJsonAsync(rootApiMessage);
        });
    }
}