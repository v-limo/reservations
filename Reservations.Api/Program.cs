using Microsoft.EntityFrameworkCore;
using Reservations.Api.Data;
using Reservations.Api.Middleware;
using Reservations.Api.Profiles;
using Reservations.Api.Services.Implementation;
using Reservations.Api.Services.Interfaces;
// using Npgsql.EntityFrameworkCore.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);
var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var PostgresConnection = builder.Configuration.GetConnectionString("PostgresConnection");


// Add services to the container.
// builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(DefaultConnection));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(PostgresConnection));

builder.Services.AddTransient<ErrorHandlerMiddleware>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
if (true) //TODO: enanble swagger only in dev
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.Run();
