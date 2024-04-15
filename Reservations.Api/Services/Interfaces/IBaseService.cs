namespace Reservations.Api.Services.Interfaces;

public interface IBaseService<TDto, in TCreateDto, in TUpdateDto>
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto> GetByIdAsync(int id);
    Task<TDto> CreateAsync(TCreateDto createDto);
    Task<TDto?> UpdateAsync(int id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(int id);
}