namespace Reservations.API.Services;


class BaseService<TEntity, TDto, TDbContext> : IBaseService<TEntity, TDto>
    where TEntity : class
    where TDto : class
    where TDbContext : DbContext
{
  
}
