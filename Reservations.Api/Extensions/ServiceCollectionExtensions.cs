namespace Reservations.Api.Extensions;

public abstract class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public static class ServiceCollectionExtensions
{
    public static void ConfigureSqliteDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
    }

    public static void ConfigureCors(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }

    public static void ConfigureMiddlewareAndServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ErrorHandlerMiddleware>();
        serviceCollection.AddRouting(options => options.LowercaseUrls = true);
        serviceCollection.AddScoped<IBookService, BookService>();
        serviceCollection.AddControllers();
        serviceCollection.AddAutoMapper(typeof(MappingProfile));
        serviceCollection.AddEndpointsApiExplorer();
    }

    public static void ConfigureSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(
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
    }

    public static IQueryable<T> Paging<T>(this IQueryable<T> query, PaginationParams paginationParams)
    {
        return query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize);
    }

    public static IQueryable<T> Filtering<T>(this IQueryable<T> query, string search)
    {
        return query.Where(arg => true);
    }

    public static IQueryable<T> Searching<T>(this IQueryable<T> query, string search)
    {
        return query;
    }
}