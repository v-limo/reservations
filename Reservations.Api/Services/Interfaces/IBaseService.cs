namespace Reservations.Api.Services.Interfaces;

public interface IBaseService<TDto, CreateDto, UpdateDto>
    where TDto : class
    where CreateDto : class
    where UpdateDto : class
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(int id);
    Task<TDto> CreateAsync(CreateDto createDto);
    Task<TDto?> UpdateAsync(int id, UpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}
