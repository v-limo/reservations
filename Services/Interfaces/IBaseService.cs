namespace Reservations.API.Services;


public interface IBaseService<Tentity, Tdto>
    where TEntity : class
    where TDto : class
{
  Task<TDto> GetByIdAsync(int id);
  Task<IEnumerable<TDto>> GetAllAsync();
  Task<TDto> CreateAsync(TDto dto);
  Task<TDto> UpdateAsync(int id, TDto dto);
  Task<bool> DeleteAsync(int id);
}
