namespace Reservations.Api.Services.Interfaces;

public interface IBaseService<TDto>
    where TDto : class
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(int id);
    Task<TDto> CreateAsync(TDto createDto);
    Task<TDto?> UpdateAsync(int id, TDto updateDto);
    Task<bool> DeleteAsync(int id);
}
