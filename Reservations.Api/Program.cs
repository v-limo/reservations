var builder = WebApplication.CreateBuilder(args);

var sqliteConnection = builder.Configuration.GetConnectionString("SqliteConnection") ?? string.Empty;

builder.Services.ConfigureSqliteDatabase(sqliteConnection);
builder.Services.ConfigureMiddlewareAndServices();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCors();

var app = builder.Build();

await app.ApplyDatabaseMigrations();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseCors();
app.MapControllers();

app.Run();