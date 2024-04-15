using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var postgresConnection = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(postgresConnection));

builder.Services.AddTransient<ErrorHandlerMiddleware>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Reservations API",
            Version = "v1",
            Description = "Reservations API for the book store",
            Contact = new OpenApiContact { Name = "Vincent Limo", Email = "limovincenti@gmail.com" },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    // TODO: use this only in development
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors();
app.MapControllers();

app.MapGet("/", () => "Return null or nothing ");
app.Run();