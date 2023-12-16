var builder = WebApplication.CreateBuilder(args);
var sqliteConnection = builder.Configuration.GetConnectionString("SqliteConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(sqliteConnection);
}
);

builder.Services.AddTransient<ErrorHandlerMiddleware>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "Reservations API",
            Version = "v1",
            Description = "Reservations API for the book store",
            Contact = new() { Name = "Vincent Limo", Email = "limovincenti@gmail.com" },
            License = new()
            {
                Name = "MIT",
                Url = new("https://opensource.org/licenses/MIT")
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

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();
